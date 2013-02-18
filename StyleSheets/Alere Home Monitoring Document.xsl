<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:ms="urn:schemas-microsoft-com:xslt"
 version="1.0">
  <xsl:template match="Document">

    <table border="0" cellpadding="0" cellspacing="0" width="800">
      <tr>
        <td width="800">
          <xsl:apply-templates select="Header"/>
          <xsl:apply-templates select="Details1" />
        </td>
      </tr>
    </table>


  </xsl:template>

  <xsl:template match="Header">
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
      <tr>
        <td align="left" valign="top" width="1000">
          <table border="0" cellpadding="0" cellspacing="0"
              style="FONT-FAMILY: Arial, Helvetica, Geneva, Swiss, SunSans-Regular; COLOR: #333333; FONT-SIZE: 12px"
              width="100%">
            <tr height="20">
              <td bgcolor="#666666" height="20" width="1000">
                <b>
                  <span style="COLOR: #ffffff"> Home Monitoring Document Order Info</span>
                </b>
              </td>
            </tr>
            <tr height="10" valign="top">
              <td height="10" width="100%">
              </td>
            </tr>
            <tr height="162" valign="top">
              <td height="162" width="100%">
                <b>Order ID: </b>
                <xsl:value-of select="OrderID"/>
                <br />
                <b>Patient ID: </b>
                <xsl:value-of select="PatientID"/>
                <br />
                <br />
                <table border="0" cellpadding="0" cellspacing="0"
                    style="FONT-FAMILY: Arial, Helvetica, Geneva, Swiss, SunSans-Regular; COLOR: #333333; FONT-SIZE: 12px"
                    width="344">
                  <tr valign="top" colspan="2">
                    <td height="112" width="100%">
                      <b>
                        <span style="COLOR: #333333"> Basic Patient Info </span>
                      </b><br />
                      Name: <xsl:value-of select="FirstName"/>  &#160;<xsl:value-of select="LastName"/> <br />
                      Date Of Birth: <xsl:value-of select="ms:format-date(DOB, 'MM-dd-yyyy')"/><br />
                      Customer Class:<xsl:value-of select="CustomerClass"/><br />
                      Last Ship Date:<xsl:value-of select="ms:format-date(LastShipDate, 'MM-dd-yyyy')"/><br />
                      Meter Type:<xsl:value-of select="MeterType"/><br />
                      Lancet Type:<xsl:value-of select="LancetType"/><br />
                      Notes: <xsl:value-of select="BasicInfoNoteColumn"/><br /><br /><br />
                    </td>
                    <td height="112" width="12"></td>
                    <td height="112" width="100%">
                      <br />
                    </td>
                  </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0"
                      style="FONT-FAMILY: Arial, Helvetica, Geneva, Swiss, SunSans-Regular; COLOR: #333333; FONT-SIZE: 12px"
                      width="100%">
                  <tr valign="top">
                    <td height="112" width="100%">
                      <b>
                        <span style="COLOR: #333333"> Insurance Info </span>
                      </b><br />
                      Primary Plan: <xsl:value-of select="PrimaryPlan"/><br />
                      Primary Policy Number: <xsl:value-of select="PrimaryPolicyNumber"/><br />
                      Primary Group Number<xsl:value-of select="PrimaryGroupNumber"/><br />
                      Primary Claim Phone Number:<xsl:value-of select="PrimaryClaimPhoneNumber"/><br />
                      Primary Address:<xsl:value-of select="PrimaryAddress"/><br />
                      Primary City:<xsl:value-of select="PrimaryCity"/><br />
                      Primary Effectivity Date: <xsl:value-of select="ms:format-date(PrimaryEffectivityDate, 'MM-dd-yyyy')"/><br /><br />
                      Secondary Plan: <xsl:value-of select="SecondaryPlan"/><br />
                      Secondary Policy Number: <xsl:value-of select="SecondaryPolicyNumber"/><br />
                      Secondary Group Number<xsl:value-of select="SecondaryGroupNumber"/><br />
                      Secondary Claim Phone Number:<xsl:value-of select="SecondaryClaimPhoneNumber"/><br />
                      Secondary Address:<xsl:value-of select="SecondaryAddress"/><br />
                      Secondary City:<xsl:value-of select="SecondaryCity"/><br />
                      Secondary Effectivity Date: <xsl:value-of select="ms:format-date(SecondaryEffectivityDate, 'MM-dd-yyyy')"/><br /><br />
                      Supply Order:<xsl:value-of select="SupplyOrder"/><br />
                      Notes: <xsl:value-of select="InsuranceInfoNoteColumn"/><br /><br /><br />
                    </td>
                    <td height="112" width="100%">
                      <br />
                    </td>
                  </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0"
                      style="FONT-FAMILY: Arial, Helvetica, Geneva, Swiss, SunSans-Regular; COLOR: #333333; FONT-SIZE: 12px"
                      width="100%">
                  <tr valign="top">
                    <td height="112" width="100%">
                      <b>
                        <span style="COLOR: #333333"> Doctor Info </span>
                      </b><br />
                      Name: <xsl:value-of select="DoctorName"/><br />
                      Phone: <xsl:value-of select="DoctorPhone"/><br />
                      City<xsl:value-of select="DoctorCity"/><br />
                      State:<xsl:value-of select="DoctorState"/><br />
                      Zip:<xsl:value-of select="DoctorZip"/><br />
                      Clinic:<xsl:value-of select="Clinic"/><br />
                      Clinic Code: <xsl:value-of select="ClinicCode"/><br />
                      Notes: <xsl:value-of select="DoctorInfoNoteColumn"/><br /><br /><br />
                    </td>
                    <td height="112" width="100%">
                      <br />
                    </td>
                  </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0"
                      style="FONT-FAMILY: Arial, Helvetica, Geneva, Swiss, SunSans-Regular; COLOR: #333333; FONT-SIZE: 12px"
                      width="100%">
                  <tr valign="top">
                    <td height="112" width="100%">
                      <b>
                        <span style="COLOR: #333333"> Patient Address Info </span>
                      </b><br />
                      Address ID: <xsl:value-of select="PatientAddressID"/><br />
                      Address: <xsl:value-of select="PatientAddress"/><br />
                      City<xsl:value-of select="PatientCity"/><br />
                      State:<xsl:value-of select="PatientState"/><br />
                      Zip:<xsl:value-of select="PatientZip"/><br />
                      Phone:<xsl:value-of select="PatientPhone"/><br />
                      E-Mail:<xsl:value-of select="EMail"/><br />
                      Notes: <xsl:value-of select="PatientInfoNoteColumn"/><br /><br /><br />
                    </td>
                    <td height="112" width="100%">
                      <br />
                    </td>
                  </tr>
                </table>
                <table border="0" cellpadding="0" cellspacing="0"
                      style="FONT-FAMILY: Arial, Helvetica, Geneva, Swiss, SunSans-Regular; COLOR: #333333; FONT-SIZE: 12px"
                      width="100%">
                  <tr valign="top">
                    <td height="112" width="100%">
                      <b>
                        <span style="COLOR: #333333"> Order Info </span>
                      </b><br />
                      No Supplies Needed? <xsl:value-of select="SuppliesNotNeeded"/><br />
                      Order Notes: <xsl:value-of select="NoteColumn"/><br /><br /><br />

                    </td>
                    <td height="112" width="100%">
                      <br />
                    </td>
                  </tr>
                </table>
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>

  </xsl:template>
  <xsl:template match="Details1">
    <b>
      <span style="FONT-FAMILY: Arial, Helvetica, Geneva, Swiss, SunSans-Regular; COLOR: #333333; FONT-SIZE: 12px"> Order Info </span>
    </b>
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
      <tr>
        <td align="left" valign="top" width="100%">
          <table border="0" cellpadding="0" cellspacing="0"
              style="FONT-FAMILY: Arial, Helvetica, Geneva, Swiss, SunSans-Regular; COLOR: #333333; FONT-SIZE: 12px"
              width="100%">
            <tr height="10" valign="bottom">
              <td colspan="7" height="10" style="BORDER-BOTTOM: 1px solid" valign="bottom"></td>
            </tr>
            <tr height="20">
              <td bgcolor="#dddddd" height="20"
                  style="PADDING-BOTTOM: 1px; MARGIN-BOTTOM: 4px" valign="center">
                <b>
                  Item SKU<br />
                </b>
              </td>
              <td bgcolor="#dddddd" height="20"
                  style="PADDING-BOTTOM: 1px; MARGIN-BOTTOM: 4px" valign="center">
                <b>
                  Description<br />
                </b>
              </td>
              <td align="left" bgcolor="#dddddd" height="20"
                  style="PADDING-BOTTOM: 1px; MARGIN-BOTTOM: 4px" valign="center" width="65">
                <b>
                  Qty<br />
                </b>
              </td>
            </tr>
            <xsl:for-each select="LineItem">
              <tr>
                <td style="PADDING-BOTTOM: 5px; MARGIN-BOTTOM: 5px; BORDER-TOP: #333333 1px dashed; PADDING-TOP: 7px">
                  <xsl:value-of select="ItemSKU"/>
                </td>
                <td style="PADDING-BOTTOM: 5px; MARGIN-BOTTOM: 5px; BORDER-TOP: #333333 1px dashed; PADDING-TOP: 7px">
                  <xsl:value-of select="Description"/>
                </td>
                <td style="PADDING-BOTTOM: 5px; MARGIN-BOTTOM: 5px; BORDER-TOP: #333333 1px dashed; PADDING-TOP: 7px">
                  <xsl:value-of select="format-number(Quantity,'##.00')"/>
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
  </xsl:template>
</xsl:stylesheet>
