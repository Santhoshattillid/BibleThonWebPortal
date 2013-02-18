<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:ms="urn:schemas-microsoft-com:xslt"
 version="1.0">
  <xsl:template match="Document">

    <table border="0" cellpadding="0" cellspacing="0" width="600">
      <tr>
        <td width="600">
          <xsl:apply-templates select="Header"/>
          <xsl:apply-templates select="Details1" />
        </td>
      </tr>
    </table>


  </xsl:template>

  <xsl:template match="Header">
    <table border="0" cellpadding="0" cellspacing="0" width="600">
      <tr>
        <td align="left" valign="top" width="360">
          <table border="0" cellpadding="0" cellspacing="0"
              style="FONT-FAMILY: Arial, Helvetica, Geneva, Swiss, SunSans-Regular; COLOR: #333333; FONT-SIZE: 12px"
              width="360">
            <tr height="20">
              <td bgcolor="#666666" height="20" width="360">
                <b>
                  <span style="COLOR: #ffffff"> Chargeback Details</span>
                </b>
              </td>
            </tr>
            <tr height="10" valign="top">
              <td height="10" width="344">
              </td>
            </tr>
            <tr height="162" valign="top">
              <td height="162" width="344">
                <b>Account Name: </b>
                <xsl:value-of select="AccountName"/>
                <br />
                <b>Account Number: </b>
                <xsl:value-of select="GAIAMAccountNumber"/>                
                <br />
                <b>Indirect Account Name: </b>
                <xsl:value-of select="IndirectAccountName"/>
                <table border="0" cellpadding="0" cellspacing="0"
                    style="FONT-FAMILY: Arial, Helvetica, Geneva, Swiss, SunSans-Regular; COLOR: #333333; FONT-SIZE: 12px"
                    width="344">
                  <tr valign="top">
                    <td height="112" width="166">
                      <b>Billing Address:</b><br />
                      <xsl:value-of select="Contact"/><br />
                      <xsl:value-of select="AddressStreet"/><br />
                      <xsl:value-of select="AddressCity"/>, <xsl:value-of select="AddressState"/> <xsl:value-of select="AddressZip"/>
                      <br />
                      <br />
                      <xsl:value-of select="Phone"/>
                    </td>
                    <td height="112" width="12"></td>
                    <td height="112" width="166"></td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
        </td>
        <td align="left" valign="top" width="240">
          <table border="0" cellpadding="0" cellspacing="0"
              style="FONT-FAMILY: Arial, Helvetica, Geneva, Swiss, SunSans-Regular; COLOR: #333333; FONT-SIZE: 12px"
              width="240">
            <tr height="20">
              <td bgcolor="#666666" height="20">
                <b>
                  <span style="COLOR: #ffffff"></span>
                </b>
              </td>
            </tr>
            <tr height="10" valign="top">
              <td height="10">
              </td>
            </tr>
            <tr height="162" valign="top">
              <td height="162">
                <b>Sales Rep: </b><xsl:value-of select="SalesPerson"/><br />                                
                <b>ExpenseType: </b><xsl:value-of select="ExpenseType"/><br />
                <br />
                <b>Total Amount: </b>$<xsl:value-of select="format-number(TotalAmount,'##.00')"/><br />
              </td>
            </tr>
          </table>
        </td>
      </tr>
      <tr>
        <td align="left" colspan="3" valign="top" width="600">
          <table bgcolor="#666666" border="0" cellpadding="2" cellspacing="0"
              style="FONT-FAMILY: Arial, Helvetica, Geneva, Swiss, SunSans-Regular; COLOR: #ffffff; FONT-SIZE: 12px"
              width="600">
            <tr>
              <td colspan="2">
                <b>Auth #: </b>
                <xsl:value-of select="AuthorizationNumber"/>
              </td>
              <td>
                <b>Start: </b>
                <xsl:value-of select="ms:format-date(DateStartEvent, 'MMM dd, yyyy')"/>
              </td>
              <td align="right" colspan="2">
                <b>End: </b>
                <xsl:value-of select="ms:format-date(DateEndEvent, 'MMM dd, yyyy')"/>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
  </xsl:template>

  <xsl:template match="Details1">
    <div id="contentd" style="position: relative; top:
0px;left: 0px; height: 600px; overflow-y: scroll;overflow-x: hidden;">
    <table border="0" cellpadding="0" cellspacing="0" width="600">
      <tr>
        <td align="left" valign="top" width="600">
          <table border="0" cellpadding="0" cellspacing="0"
              style="FONT-FAMILY: Arial, Helvetica, Geneva, Swiss, SunSans-Regular; COLOR: #333333; FONT-SIZE: 12px"
              width="600">
            <tr height="10" valign="bottom">
              <td colspan="7" height="10" style="BORDER-BOTTOM: 1px solid" valign="bottom"></td>
            </tr>
            <tr height="20">
              <td bgcolor="#dddddd" height="20"
                  style="PADDING-BOTTOM: 1px; MARGIN-BOTTOM: 4px" valign="center">
                <b>
                  Item Number<br />
                </b>
              </td>
              <td bgcolor="#dddddd" height="20"
                    style="PADDING-BOTTOM: 1px; MARGIN-BOTTOM: 4px" valign="center">
                <b>
                  UPC Code<br />
                </b>
              </td>
              <td bgcolor="#dddddd" height="20"
                  style="PADDING-BOTTOM: 1px; MARGIN-BOTTOM: 4px" valign="center">
                <b>
                  Title<br />
                </b>
              </td>
              <td bgcolor="#dddddd" height="20"
                  style="PADDING-BOTTOM: 1px; MARGIN-BOTTOM: 4px" valign="center">
                <b>
                  Studio<br />
                </b>
              </td>
              <td bgcolor="#dddddd" height="20"
                  style="PADDING-BOTTOM: 1px; MARGIN-BOTTOM: 4px" valign="center">
                <b>
                  Unit Charge<br />
                </b>
              </td>
              <td align="left" bgcolor="#dddddd" height="20"
                  style="PADDING-BOTTOM: 1px; MARGIN-BOTTOM: 4px" valign="center" width="65">
                <b>
                  Qty<br />
                </b>
              </td>
              <td align="left" bgcolor="#dddddd" height="20"
                  style="PADDING-BOTTOM: 1px; MARGIN-BOTTOM: 4px" valign="center" width="65">
                <b>
                  Total<br />
                </b>
              </td>
            </tr>
            <xsl:for-each select="LineItem">
              <tr>
                <td style="PADDING-BOTTOM: 5px; MARGIN-BOTTOM: 5px; BORDER-TOP: #333333 1px dashed; PADDING-TOP: 7px">
                  <xsl:value-of select="GAIAMItemNumber"/>
                </td>                
                <td style="PADDING-BOTTOM: 5px; MARGIN-BOTTOM: 5px; BORDER-TOP: #333333 1px dashed; PADDING-TOP: 7px">
                  <xsl:value-of select="UPCCode"/>
                </td>
                <td style="PADDING-BOTTOM: 5px; MARGIN-BOTTOM: 5px; BORDER-TOP: #333333 1px dashed; PADDING-TOP: 7px">
                  <xsl:value-of select="TitlePromotion"/>
                </td>
                <td style="PADDING-BOTTOM: 5px; MARGIN-BOTTOM: 5px; BORDER-TOP: #333333 1px dashed; PADDING-TOP: 7px">
                  <xsl:value-of select="Studio"/>
                </td>
                <td style="PADDING-BOTTOM: 5px; MARGIN-BOTTOM: 5px; BORDER-TOP: #333333 1px dashed; PADDING-TOP: 7px">
                  $<xsl:value-of select="format-number(AmountPerUnit,'##.00')"/>
                </td>
                <td style="PADDING-BOTTOM: 5px; MARGIN-BOTTOM: 5px; BORDER-TOP: #333333 1px dashed; PADDING-TOP: 7px">
                  <xsl:value-of select="format-number(Quantity,'##.00')"/>
                </td>
                <td style="PADDING-BOTTOM: 5px; MARGIN-BOTTOM: 5px; BORDER-TOP: #333333 1px dashed; PADDING-TOP: 7px"
                    width="65">
                  $<xsl:value-of select="format-number(TotalAmount,'##.00')"/>
                </td>
              </tr>
            </xsl:for-each>
            <tr height="4">
              <td height="4" style="BORDER-TOP: 1px solid">
              </td>
              <td height="4" style="BORDER-TOP: 1px solid" valign="top">
              </td>
              <td height="4" style="BORDER-TOP: 1px solid" valign="top">
              </td>
              <td height="4" style="BORDER-TOP: 1px solid" valign="top">
              </td>
              <td height="4" style="BORDER-TOP: 1px solid" valign="top">
              </td>
              <td height="4" style="BORDER-TOP: 1px solid" valign="top">
              </td>
              <td align="right" height="4" style="BORDER-TOP: 1px solid" valign="top" >
              </td>
            </tr>
            <tr height="20">
              <td height="20">
                <br />
              </td>
              <td height="20" valign="top">
                <br />
              </td>
              <td height="20" valign="top">
                <br />
              </td>
              <td height="20" valign="top">
                <br />
              </td>
              <td height="20" valign="top">
                <br />
              </td>
              <td height="20" valign="top">
                <br />
              </td>
              <td align="right" height="20" valign="top">
                <br />
              </td>
            </tr>
            <tr height="19">
              <td colspan="7" height="19" style="BORDER-TOP: 1px solid">
                <br />
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>
    </div>
  </xsl:template>
</xsl:stylesheet>
