using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using System.Collections;

/// <summary>
/// Summary description for GPCustomer
/// </summary>
    #region GPGLItem Class
    public class GPGLItem
    {
        #region Private Variables

        private string batchNumber = string.Empty;
        private int journalEntry = 0;
        private decimal sqncline = 0;
        private int actIndx = 0;
        private decimal creditAmount = 0;
        private decimal debitAmount = 0;
        private string actNumSt = string.Empty;
        private string description = string.Empty;
        private DateTime docDate = Convert.ToDateTime(System.DateTime.Today.ToShortDateString());


        //used for checking
        private decimal actAmount = 0;

        #endregion

        #region Public Variables

        public string BatchNumber { get { return batchNumber; } set { batchNumber = value; } }
        public int JournalEntry { get { return journalEntry; } set { journalEntry = value; } }
        public decimal SqncLine { get { return sqncline; } set { sqncline = value; } }
        public int ActIndx { get { return actIndx; } set { actIndx = value; } }
        public decimal CreditAmount { get { return creditAmount; } set { creditAmount = value; } }
        public decimal DebitAmount { get { return debitAmount; } set { debitAmount = value; } }
        public string ActNumSt { get { return actNumSt; } set { actNumSt = value; } }
        public string Description { get { return description; } set { description = value; } }
        public DateTime DocDate { get { return docDate; } set { docDate = value; } }
        public decimal ActAmount { get { return actAmount; } set { actAmount = value; } }

        #endregion

        #region Constructor
        public GPGLItem()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        #endregion
    }
    #endregion

    #region GLItemList Class
    public class GPGLItemList : SortedList, IList
    {
        #region Private Variables
        #endregion

        #region Public Properties
        public override bool IsFixedSize { get { return base.IsFixedSize; } }
        public override bool IsReadOnly { get { return base.IsReadOnly; } }
        #endregion

        #region Indexers
        public object this[int index] { get { try { return base.GetByIndex(index); } catch (Exception) { return null; } } set { Add(value); } }
        public GPGLItem this[string key] { get { return (GPGLItem)base[key]; } }
        #endregion

        #region Constructor
        public GPGLItemList() : base() { }
        #endregion

        #region IList Interface implemented Methods
        public int Add(object GPGLItem) { base.Add(((GPGLItem)GPGLItem).ActIndx, GPGLItem); return IndexOf(GPGLItem); }
        public override void Clear() { base.Clear(); }
        public override bool Contains(object GPGLItem) { return base.Contains(GPGLItem); }
        public int IndexOf(object GPGLItem) { return base.IndexOfKey(((GPGLItem)GPGLItem).ActIndx); }
        public void Insert(int index, object GPGLItem) { Add(GPGLItem); }
        public override void Remove(object GPGLItem) { base.Remove(((GPGLItem)GPGLItem).ActIndx); }
        public override void RemoveAt(int index) { base.RemoveAt(index); }        
        #endregion

        #region Custom Methods
        public void Add(GPGLItem GPGLItem)
        {
            if (GPGLItem.ActIndx.ToString().Length != 0)
            {
                if (base.Contains(GPGLItem.ActIndx))
                    base.Remove(GPGLItem.ActIndx);
                base.Add(GPGLItem.ActIndx, GPGLItem);
            }
            else
                base.Add(System.Guid.NewGuid().ToString(), GPGLItem);
        }

        public void Update(GPGLItem GPGLItem)
        {
            base.Remove(GPGLItem.ActIndx);
            base.Add(GPGLItem.ActIndx, GPGLItem);
        }

        #endregion
    }
    #endregion
