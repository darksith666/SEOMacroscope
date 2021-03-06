﻿/*

  This file is part of SEOMacroscope.

  Copyright 2019 Jason Holland.

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
using System.Collections.Generic;
using ClosedXML.Excel;

namespace SEOMacroscope
{

  public partial class MacroscopeExcelRedirectsReport : MacroscopeExcelReports
  {

    /**************************************************************************/

    private void BuildWorksheetPageRedirectChains (
      MacroscopeJobMaster JobMaster,
      XLWorkbook wb,
      string WorksheetLabel
    )
    {
      var ws = wb.Worksheets.Add( WorksheetLabel );

      int iRow = 1;
      int iCol = 1;
      int iColMax = 1;

      MacroscopeDocumentCollection DocCollection = JobMaster.GetDocCollection();
      MacroscopeAllowedHosts AllowedHosts = JobMaster.GetAllowedHosts();
      List<List<MacroscopeRedirectChainDocStruct>> RedirectChains = DocCollection.GetMacroscopeRedirectChains();

      {
        ws.Cell( iRow, iCol ).Value = "Hop";
        iCol++;
        ws.Cell( iRow, iCol ).Value = "Status";
      }

      iRow++;

      foreach ( List<MacroscopeRedirectChainDocStruct> DocList in RedirectChains )
      {

        int iHop = 1;

        iCol = 1;

        foreach ( MacroscopeRedirectChainDocStruct RedirectChainDocStruct in DocList )
        {

          string Url = RedirectChainDocStruct.Url;
          string StatusCode = RedirectChainDocStruct.StatusCode.ToString();

          ws.Cell( 1, iCol ).Value = string.Format( "Hop {0} URL", iHop );
          this.InsertAndFormatUrlCell( ws, iRow, iCol, Url );
          iCol++;

          if ( AllowedHosts.IsInternalUrl( Url: Url ) )
          {
            ws.Cell( iRow, iCol ).Style.Font.SetFontColor( XLColor.Green );
          }
          else
          {
            ws.Cell( iRow, iCol ).Style.Font.SetFontColor( XLColor.Gray );
          }

          ws.Cell( 1, iCol ).Value = string.Format( "Hop {0} Status", iHop );
          this.InsertAndFormatContentCell( ws, iRow, iCol, StatusCode );
          iCol++;

          iHop++;

        }

        if ( iCol > iColMax )
        {
          iColMax = iCol;
        }

        iRow++;

      }

      if ( ( iRow > 1 ) && ( iColMax > 2 ) )
      {
        var rangeData = ws.Range( 1, 1, iRow - 1, iColMax - 1 );
        var excelTable = rangeData.CreateTable();
      }

    }

    /**************************************************************************/

  }

}
