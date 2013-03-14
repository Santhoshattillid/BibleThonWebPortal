using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for CustomerInetInfo
/// </summary>
public class CustomerInetInfo
{
	public CustomerInetInfo()
	{
		//
		// TODO: Add constructor logic here
		//
	}

    public string MasterType { get; set; }
    public string MasterId { get; set; }
    public string AddressCode { get; set; }
    public string IntenetInfo1 { get; set; }
    public string IntenetInfo2 { get; set; }
    public string IntenetInfo3 { get; set; }
    public string IntenetInfo4 { get; set; }
    public string EmailToAddress { get; set; }
    public string EmailBccAddress { get; set; }
    public string EmailCcAddress { get; set; }	
}