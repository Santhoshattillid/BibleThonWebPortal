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

  <xsl:template match="Header" >
    <table border="0" cellpadding="0" cellspacing="0" width="100%">
      <tr>
        <td align="left" valign="top" width="1000">
          <table border="0" cellpadding="0" cellspacing="0"
              style="FONT-FAMILY: Arial, Helvetica, Geneva, Swiss, SunSans-Regular; COLOR: #333333; FONT-SIZE: 18px"
              width="100%">
            <tr height="20">
              <td bgcolor="#666666" height="20" width="1000">
                <b>
                  <span style="COLOR: #ffffff"> Supply Callback Document</span>
                </b>
              </td>
            </tr>
            <tr height="10" valign="top">
              <td height="10" width="100%">
              </td>
            </tr>
            <tr height="162" valign="top">
              <td height="162" width="100%">
                <b>Batch ID: </b>
                <xsl:value-of select="BatchID"/>
                <br />
                <b>Member ID: </b>
                <xsl:value-of select="MemberID"/>
                <br />
                <b>Session ID: </b>
                <xsl:value-of select="SessionID"/>
                <br />
                <b>Date Row Inserted: </b>
                <xsl:value-of select="ms:format-date(DateRowInserted, 'MM-dd-yyyy')"/>
                <br />
                <b>Call Time: </b>
                <xsl:value-of select="ms:format-time(CallTime, 'hh:mm:ss')"/>
                <br />
                <b>Call Result: </b>
                <xsl:value-of select="CallResult"/>
                <br />
                <b>TOD_Supplies: </b>
                <xsl:value-of select="TOD_Supplies"/>
                <br />
                <b>Member Number: </b>
                <xsl:value-of select="MemberNumber"/>
                <br />
                <b>Name: </b>
                <xsl:value-of select="FirstName"/>  &#160;<xsl:value-of select="LastName"/> <br />
                <b>Date Of Birth: </b>
                <xsl:value-of select="ms:format-date(BirthDate, 'MM-dd-yyyy')"/> <br />
                <b>Phone Number: </b>
                <xsl:value-of select="PhoneNumber"/>
                <br />
                <b>DateProcessed: </b>
                <xsl:value-of select="ms:format-date(DateProcessed, 'MM-dd-yyyy')"/>
                <br />
              </td>
            </tr>
          </table>
        </td>
      </tr>
    </table>

  </xsl:template>
</xsl:stylesheet>
