  
 #region 大量資料匯入
        private static ResultMsg InsertSourceData(List<Source> data)
        {
            //convert to datatable
            DataTable dt = clsdt.ToDataTable(data, true);
            try
            {
                var columnMapping = new List<string>();
                //columnMapping.Add("SystemId,SystemId");
                columnMapping.Add("FileSystemId,RfFile");             
                columnMapping.Add("ChipNo,ChipNo");
                columnMapping.Add("TxnDate,TxnDate");
                columnMapping.Add("TxnTime,TxnTime");        
                columnMapping.Add("SourceFile,SourceFile");
                columnMapping.Add("Multiple,Multiple");
                columnMapping.Add("Status,Status");
                msg = clsdt.BulkInsert(dt, "Source", columnMapping);
            }
            catch (Exception ex)
            {
                msg.Status = false;
                msg.Message = ex.Message;
            }

            return msg;
        }
   #endregion

    //Bulkinsert Class
        public ResultMsg BulkInsert(DataTable dt, String TableName, List<string> columnMapping)
        {
            ResultMsg msg = new ResultMsg();
            try
            {
                using (SqlConnection connection = new SqlConnection(Global.connectionString))
                {
                    // make sure to enable triggers
                    // more on triggers in next post
                    SqlBulkCopy bulkCopy =
                        new SqlBulkCopy
                        (
                        connection,
                        SqlBulkCopyOptions.TableLock |
                        SqlBulkCopyOptions.FireTriggers |
                        SqlBulkCopyOptions.UseInternalTransaction,
                        null
                        );

                    // set the destination table name
                    bulkCopy.DestinationTableName = TableName;
                    foreach (var mapping in columnMapping)
                    {
                        var split = mapping.Split(new[] { ',' });
                        bulkCopy.ColumnMappings.Add(split.First(), split.Last());
                    }

                    connection.Open();
                    // write the data in the "dataTable"
                    bulkCopy.WriteToServer(dt);
                    msg.Status = true;
                    msg.Message = String.Format("{0}匯入成功,共{1}筆", TableName, dt.Rows.Count);

                    connection.Close();

                }
            }
            catch (Exception ex)
            {
                msg.Status = false;
                msg.Message = ex.Message;
            }
            return msg;
        }

        //List to dttable  Class

         public DataTable ToDataTable<T>(List<T> items,Boolean IsFilter=false)
        {
            DataTable dataTable = null;
            try
            {
             dataTable = new DataTable(typeof(T).Name);
            //Get all the properties by using reflection   
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            IList<string> strList = new List<string>() { "SystemId", "FileSystemId"};
            
                if (IsFilter)
                {
                    foreach (PropertyInfo prop in Props)
                    {
                        //Setting column names as Property names  
                        if (strList.Contains(prop.Name))
                        {
                            dataTable.Columns.Add(prop.Name, typeof(Guid));
                        }
                        else
                        { dataTable.Columns.Add(prop.Name); }
                    }
                }
                else
                {
                    foreach (PropertyInfo prop in Props)
                    {
                        //Setting column names as Property names                       
                         dataTable.Columns.Add(prop.Name); 
                    }
                }
          
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {

                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine(string.Format("{0},ListToDataTable發生錯誤,{1}", DateTime.Now, ex.Message));
            }
            return dataTable;
        }