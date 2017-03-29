﻿/*

  This file is part of SEOMacroscope.

  Copyright 2017 Jason Holland.

  The GitHub repository may be found at:

    https://github.com/nazuke/SEOMacroscope

  Foobar is free software: you can redistribute it and/or modify
  it under the terms of the GNU General Public License as published by
  the Free Software Foundation, either version 3 of the License, or
  (at your option) any later version.

  Foobar is distributed in the hope that it will be useful,
  but WITHOUT ANY WARRANTY; without even the implied warranty of
  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
  GNU General Public License for more details.

  You should have received a copy of the GNU General Public License
  along with Foobar.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using ClosedXML.Excel;

namespace SEOMacroscope
{

  public partial class MacroscopeExcelPageContentsReport : MacroscopeExcelReports
  {

    /**************************************************************************/

    private void BuildWorksheetPageTitles (
      MacroscopeJobMaster JobMaster,
      XLWorkbook wb,
      string sWorksheetLabel
    )
    {
      var ws = wb.Worksheets.Add( sWorksheetLabel );

      int iRow = 1;
      int iCol = 1;
      int iColMax = 1;

      MacroscopeDocumentCollection DocCollection = JobMaster.GetDocCollection();

      {

        ws.Cell( iRow, iCol ).Value = "URL";
        iCol++;

        ws.Cell( iRow, iCol ).Value = "Occurrences";
        iCol++;

        ws.Cell( iRow, iCol ).Value = "Title";
        iCol++;
        
        ws.Cell( iRow, iCol ).Value = "Title Length";
        iCol++;

        ws.Cell( iRow, iCol ).Value = "Pixel Width";

      }

      iColMax = iCol;

      iRow++;

      foreach( string sKey in DocCollection.DocumentKeys() )
      {

        MacroscopeDocument msDoc = DocCollection.GetDocument( sKey );
        Boolean bProcess = false;

        if( msDoc.GetIsExternal() )
        {
          bProcess = false;
        }

        if( msDoc.GetIsHtml() )
        {
          bProcess = true;
        }
        else
        if( msDoc.GetIsPdf() )
        {
          bProcess = true;
        }
        else
        {
          bProcess = false;
        }

        if( bProcess )
        {

          iCol = 1;

          string Title = msDoc.GetTitle();
          int Occurrences = DocCollection.GetStatsTitleCount( Title );
          int TitleLength = msDoc.GetTitleLength();
          int TitlePixelWidth = msDoc.GetTitlePixelWidth();

          this.InsertAndFormatUrlCell( ws, iRow, iCol, msDoc );

          if( !msDoc.GetIsExternal() )
          {
            ws.Cell( iRow, iCol ).Style.Font.SetFontColor( XLColor.Green );
          }
          else
          {
            ws.Cell( iRow, iCol ).Style.Font.SetFontColor( XLColor.Gray );
          }

          iCol++;

          this.InsertAndFormatContentCell( ws, iRow, iCol, this.FormatIfMissing( Occurrences.ToString() ) );

          if( Occurrences > 1 )
          {
            ws.Cell( iRow, iCol ).Style.Font.SetFontColor( XLColor.Orange );
          }
          else
          {
            ws.Cell( iRow, iCol ).Style.Font.SetFontColor( XLColor.Green );
          }

          iCol++;

          this.InsertAndFormatContentCell( ws, iRow, iCol, this.FormatIfMissing( Title ) );

          if( TitleLength <= 0 )
          {
            ws.Cell( iRow, iCol ).Style.Font.SetFontColor( XLColor.Red );
            ws.Cell( iRow, iCol ).Value = "MISSING";
          }
          else
          {
            ws.Cell( iRow, iCol ).Style.Font.SetFontColor( XLColor.Green );
          }

          iCol++;
          
          this.InsertAndFormatContentCell( ws, iRow, iCol, this.FormatIfMissing( TitleLength.ToString() ) );

          if( TitleLength < MacroscopePreferencesManager.GetTitleMinLen() )
          {
            ws.Cell( iRow, iCol ).Style.Font.SetFontColor( XLColor.Red );
          }
          else
          if( TitleLength > MacroscopePreferencesManager.GetTitleMaxLen() )
          {
            ws.Cell( iRow, iCol ).Style.Font.SetFontColor( XLColor.Red );
          }
          else
          {
            ws.Cell( iRow, iCol ).Style.Font.SetFontColor( XLColor.Green );
          }

          iCol++;
          
          this.InsertAndFormatContentCell( ws, iRow, iCol, this.FormatIfMissing( TitlePixelWidth.ToString() ) );
          
          if( TitlePixelWidth > MacroscopePreferencesManager.GetTitleMaxPixelWidth() )
          {
            ws.Cell( iRow, iCol ).Style.Font.SetFontColor( XLColor.Red );
          }
          else
          if( TitlePixelWidth >= ( MacroscopePreferencesManager.GetTitleMaxPixelWidth() - 20 ) )
          {
            ws.Cell( iRow, iCol ).Style.Font.SetFontColor( XLColor.Red );
          }
          else
          if( TitlePixelWidth <= 0 )
          {
            ws.Cell( iRow, iCol ).Style.Font.SetFontColor( XLColor.Orange );
          }
          else
          {
            ws.Cell( iRow, iCol ).Style.Font.SetFontColor( XLColor.Green );
          }

          iRow++;
          
        }

      }

      {
        var rangeData = ws.Range( 1, 1, iRow - 1, iColMax );
        var excelTable = rangeData.CreateTable();
      }

    }

    /**************************************************************************/

  }

}
