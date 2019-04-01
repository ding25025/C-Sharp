
 // excel to datatable
 public static DataTable ReadExecelAsTable(String filename)
        { 
         using (FileStream fs = new FileStream(filename, FileMode.Open))
        {

            IWorkbook wb = new XSSFWorkbook(fs); //xlsx
           // HSSFWorkbook workbook = new HSSFWorkbook(fs);//xls

            ISheet sheet = wb.GetSheetAt(0);

         //   ISheet sheet = workbook.GetSheetAt(0);

            DataTable table = new DataTable();
            //由第一列取標題做為欄位名稱
             IRow headerRow = sheet.GetRow(0);
            int cellCount = headerRow.LastCellNum;
            for (int i = headerRow.FirstCellNum; i < cellCount; i++)
                //以欄位文字為名新增欄位，此處全視為字串型別以求簡化
                table.Columns.Add(
                    new DataColumn(headerRow.GetCell(i).StringCellValue));
            
            //略過第零列(標題列)，一直處理至最後一列
            for (int i = (sheet.FirstRowNum + 1); i <= sheet.LastRowNum; i++)
            {
                 IRow row = sheet.GetRow(i);
                if (row == null) continue;
                DataRow dataRow = table.NewRow();
                //依先前取得的欄位數逐一設定欄位內容
                for (int j = row.FirstCellNum; j < cellCount; j++)
                    if (row.GetCell(j) != null)
                        //如要針對不同型別做個別處理，可善用.CellType判斷型別
                        //再用.StringCellValue, .DateCellValue, .NumericCellValue...取值
                        //此處只簡單轉成字串
                        dataRow[j] = row.GetCell(j).ToString();
                        table.Rows.Add(dataRow);
            }
            return table;
        }
    }
