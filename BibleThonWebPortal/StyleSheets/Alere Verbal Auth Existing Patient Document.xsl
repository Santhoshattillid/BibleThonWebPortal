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
                  <span style="COLOR: #ffffff"> Verbal Auth Existing Patient Info</span>
                </b>
              </td>
            </tr>
            <tr height="10" valign="top">
              <td height="10" width="100%">
              </td>
            </tr>
            <tr height="162" valign="top">
              <td height="162" width="100%">
                <b>Record ID: </b>
                <xsl:value-of select="RecordID"/>
                <br />
                <b>Patient ID: </b>
                <xsl:value-of select="Patient_Number"/>
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
                      Name: <xsl:value-of select="Patient_First_Name"/>  &#160;<xsl:value-of select="Patient_Last_Name"/> <br />
                      Date Of Birth: <xsl:value-of select="ms:format-date(Birth_Date, 'MM-dd-yyyy')"/><br />                      
                      Doctor's Name:<xsl:value-of select="Primary_Physician_Name"/><br />
                      Doctor's Phone:<xsl:value-of select="Primary_Physician_Phone_Number"/><br />
                      Clinic:<xsl:value-of select="Referring_Source_Name"/><br />
                      Clinic Code: <xsl:value-of select="Referring_Source_Code"/><br /><br /><br />
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
                        <span style="COLOR: #333333"> Verbal Auth Existing Patient Details </span>
                      </b><br />
                      <xsl:if test="CB_Gender = 'true'">
                        <xsl:if test="Gender = 'true'">
                          Gender: Male<br />                          
                        </xsl:if>
                        <xsl:if test="Gender = 'false'">
                          Gender: Female<br />
                        </xsl:if>
                      </xsl:if>
                      <xsl:if test="CB_PrimaryPhone = 'true'">
                        Primary Phone: <xsl:value-of select="PrimaryPhone"/><br />
                      </xsl:if>
                      <xsl:if test="CB_SecondaryPhone = 'true'">
                        Secondary Phone: <xsl:value-of select="SecondaryPhone"/><br />
                      </xsl:if>
                      <xsl:if test="CB_Email = 'true'">
                        Email: <xsl:value-of select="Email"/><br />
                      </xsl:if>
                      <xsl:if test="CB_Caregiver = 'true'">
                        Caregiver: <xsl:value-of select="Caregiver"/><br />
                      </xsl:if>
                      <xsl:if test="CB_CaregiverRelationship = 'true'">
                        Caregiver Relationship: <xsl:value-of select="CaregiverRelationship"/><br />
                      </xsl:if>
                      <xsl:if test="CB_CaregiverPrimaryPhone = 'true'">
                        Caregiver Primary Phone: <xsl:value-of select="CaregiverPrimaryPhone"/><br />
                      </xsl:if>
                      <xsl:if test="CB_CaregiverSecondaryPhone = 'true'">
                        Caregiver Secondary Phone: <xsl:value-of select="CaregiverSecondaryPhone"/><br />
                      </xsl:if>
                      <xsl:if test="CB_CaregiverPOA = 'true'">
                        <xsl:if test="CaregiverPOA = 'true'">
                          POA: Yes<br />
                        </xsl:if>
                        <xsl:if test="CaregiverPOA = 'false'">
                          POA: No<br />
                        </xsl:if>
                      </xsl:if>
                      <xsl:if test="CB_Physician = 'true'">
                        Physician: <xsl:value-of select="Physician"/><br />
                      </xsl:if>
                      <xsl:if test="CB_PhysicianFax = 'true'">
                        Physician Fax: <xsl:value-of select="PhysicianFax"/><br />
                      </xsl:if>
                      <xsl:if test="CB_PhysicianPhone = 'true'">
                        Physician Phone: <xsl:value-of select="PhysicianPhone"/><br />
                      </xsl:if>
                      <xsl:if test="CB_ReportResultsTo = 'true'">
                        Report Results To: <xsl:value-of select="ReportResultsTo"/><br />
                      </xsl:if>
                      <xsl:if test="CB_ReportResultsToPhone = 'true'">
                        Report Results To Phone: <xsl:value-of select="ReportResultsToPhone"/><br />
                      </xsl:if>
                      <xsl:if test="CB_ReportResultsToFax = 'true'">
                        Report Results To Fax: <xsl:value-of select="ReportResultsToFax"/><br />
                      </xsl:if>
                      <xsl:if test="CB_AfterHoursInstruction = 'true'">
                        After Hours Instruction: <xsl:value-of select="AfterHoursInstruction"/><br />
                      </xsl:if>
                      <xsl:if test="CB_ClinicAddress = 'true'">
                        Clinic Address: <xsl:value-of select="ClinicAddress"/><br />
                      </xsl:if>
                      <xsl:if test="CB_ClinicCityStateZip = 'true'">
                        Clinic City/State/Zip: <xsl:value-of select="ClinicCityStateZip"/><br />
                      </xsl:if>
                      <xsl:if test="CB_ClinicPhone = 'true'">
                        Clinic Phone: <xsl:value-of select="ClinicPhone"/><br />
                      </xsl:if>
                      <xsl:if test="CB_MeterType = 'true'">
                        Meter Type: <xsl:value-of select="MeterType"/><br />
                      </xsl:if>
                      <xsl:if test="CB_TestFrequency = 'true'">
                        Test Frequency: <xsl:value-of select="TestFrequency"/><br />
                      </xsl:if>
                      <xsl:if test="CB_APLSDiscussion = 'true'">
                        APLSDiscussion: <xsl:value-of select="APLSDiscussion"/><br />
                      </xsl:if>
                      <xsl:if test="CB_TestReportingInstruction = 'true'">
                        Test Reporting Instruction: <xsl:value-of select="TestReportingInstruction"/><br />
                      </xsl:if>
                      <xsl:if test="CB_FaxAllResults = 'true'">                        
                        <xsl:if test="FaxAllResults = 'true'">
                          Fax All Results: Yes<br />
                        </xsl:if>
                        <xsl:if test="FaxAllResults = 'false'">
                          Fax All Results: No<br />
                        </xsl:if>
                      </xsl:if>
                      <xsl:if test="CB_MBE = 'true'">
                        <xsl:if test="MBE = 'true'">
                          MBE: Yes<br />
                        </xsl:if>
                        <xsl:if test="MBE = 'false'">
                          MBE: No<br />
                        </xsl:if>
                      </xsl:if>
                      <xsl:if test="CB_MBEDate = 'true'">
                        MBE Date: <xsl:value-of select="ms:format-date(MBEDate, 'MM-dd-yyyy')"/><br />                        
                      </xsl:if>
                      <xsl:if test="CB_SelectOptions = 'true'">
                        Select Options: <xsl:value-of select="SelectOptions"/><br />
                      </xsl:if>
                      <xsl:if test="CB_RetestIn = 'true'">
                        Retest In: <xsl:value-of select="RetestIn"/><br />
                      </xsl:if>
                      <xsl:if test="CB_OtherOptions = 'true'">
                        Other Options: <xsl:value-of select="OtherOptions"/><br />
                      </xsl:if>
                      <br/>
                      <xsl:if test="CB_AuthorizedBy = 'true'">
                        Authorized By: <xsl:value-of select="AuthorizedBy"/><br />
                      </xsl:if>
                      <xsl:if test="CB_AuthorizedByTitle = 'true'">
                        Authorized By Title: <xsl:value-of select="AuthorizedByTitle"/><br />
                      </xsl:if>
                      <xsl:if test="CB_TakenBy = 'true'">
                        Taken By: <xsl:value-of select="TakenBy"/><br />
                      </xsl:if>
                      <xsl:if test="CB_TakenByTitle = 'true'">
                        Taken By Title: <xsl:value-of select="TakenByTitle"/><br />
                      </xsl:if>
                      <xsl:if test="CB_Date = 'true'">
                        Date: <xsl:value-of select="ms:format-date(Date, 'MM-dd-yyyy')"/><br />
                      </xsl:if>
                      <xsl:if test="CB_Time = 'true'">
                        Time: <xsl:value-of select="TIME"/><br />
                      </xsl:if>
                      <br />
                      Note:  <xsl:value-of select="NoteColumn"/><br />
                      <br />
                      Details Note:  <xsl:value-of select="DetailApproverNoteColumn"/><br />
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
