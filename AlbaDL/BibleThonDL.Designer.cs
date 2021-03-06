﻿//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Serialization;

[assembly: EdmSchemaAttribute()]
namespace AlbaDL
{
    #region Contexts
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    public partial class TWOEntities : ObjectContext
    {
        #region Constructors
    
        /// <summary>
        /// Initializes a new TWOEntities object using the connection string found in the 'TWOEntities' section of the application configuration file.
        /// </summary>
        public TWOEntities() : base("name=TWOEntities", "TWOEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new TWOEntities object.
        /// </summary>
        public TWOEntities(string connectionString) : base(connectionString, "TWOEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        /// <summary>
        /// Initialize a new TWOEntities object.
        /// </summary>
        public TWOEntities(EntityConnection connection) : base(connection, "TWOEntities")
        {
            this.ContextOptions.LazyLoadingEnabled = true;
            OnContextCreated();
        }
    
        #endregion
    
        #region Partial Methods
    
        partial void OnContextCreated();
    
        #endregion
    
        #region ObjectSet Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<OrderDetail> OrderDetails
        {
            get
            {
                if ((_OrderDetails == null))
                {
                    _OrderDetails = base.CreateObjectSet<OrderDetail>("OrderDetails");
                }
                return _OrderDetails;
            }
        }
        private ObjectSet<OrderDetail> _OrderDetails;
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        public ObjectSet<AuthorizeNetTransaction> AuthorizeNetTransactions
        {
            get
            {
                if ((_AuthorizeNetTransactions == null))
                {
                    _AuthorizeNetTransactions = base.CreateObjectSet<AuthorizeNetTransaction>("AuthorizeNetTransactions");
                }
                return _AuthorizeNetTransactions;
            }
        }
        private ObjectSet<AuthorizeNetTransaction> _AuthorizeNetTransactions;

        #endregion

        #region AddTo Methods
    
        /// <summary>
        /// Deprecated Method for adding a new object to the OrderDetails EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToOrderDetails(OrderDetail orderDetail)
        {
            base.AddObject("OrderDetails", orderDetail);
        }
    
        /// <summary>
        /// Deprecated Method for adding a new object to the AuthorizeNetTransactions EntitySet. Consider using the .Add method of the associated ObjectSet&lt;T&gt; property instead.
        /// </summary>
        public void AddToAuthorizeNetTransactions(AuthorizeNetTransaction authorizeNetTransaction)
        {
            base.AddObject("AuthorizeNetTransactions", authorizeNetTransaction);
        }

        #endregion

    }

    #endregion

    #region Entities
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="TWOModel", Name="AuthorizeNetTransaction")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class AuthorizeNetTransaction : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new AuthorizeNetTransaction object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        /// <param name="cardNumber">Initial value of the CardNumber property.</param>
        /// <param name="authorizationCode">Initial value of the AuthorizationCode property.</param>
        /// <param name="invoiceNumber">Initial value of the InvoiceNumber property.</param>
        /// <param name="transactionID">Initial value of the TransactionID property.</param>
        /// <param name="message">Initial value of the Message property.</param>
        /// <param name="amount">Initial value of the Amount property.</param>
        /// <param name="approved">Initial value of the Approved property.</param>
        public static AuthorizeNetTransaction CreateAuthorizeNetTransaction(global::System.Int32 id, global::System.String cardNumber, global::System.String authorizationCode, global::System.String invoiceNumber, global::System.String transactionID, global::System.String message, global::System.Decimal amount, global::System.Boolean approved)
        {
            AuthorizeNetTransaction authorizeNetTransaction = new AuthorizeNetTransaction();
            authorizeNetTransaction.Id = id;
            authorizeNetTransaction.CardNumber = cardNumber;
            authorizeNetTransaction.AuthorizationCode = authorizationCode;
            authorizeNetTransaction.InvoiceNumber = invoiceNumber;
            authorizeNetTransaction.TransactionID = transactionID;
            authorizeNetTransaction.Message = message;
            authorizeNetTransaction.Amount = amount;
            authorizeNetTransaction.Approved = approved;
            return authorizeNetTransaction;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Int32 _Id;
        partial void OnIdChanging(global::System.Int32 value);
        partial void OnIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String CardNumber
        {
            get
            {
                return _CardNumber;
            }
            set
            {
                OnCardNumberChanging(value);
                ReportPropertyChanging("CardNumber");
                _CardNumber = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("CardNumber");
                OnCardNumberChanged();
            }
        }
        private global::System.String _CardNumber;
        partial void OnCardNumberChanging(global::System.String value);
        partial void OnCardNumberChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String AuthorizationCode
        {
            get
            {
                return _AuthorizationCode;
            }
            set
            {
                OnAuthorizationCodeChanging(value);
                ReportPropertyChanging("AuthorizationCode");
                _AuthorizationCode = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("AuthorizationCode");
                OnAuthorizationCodeChanged();
            }
        }
        private global::System.String _AuthorizationCode;
        partial void OnAuthorizationCodeChanging(global::System.String value);
        partial void OnAuthorizationCodeChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String InvoiceNumber
        {
            get
            {
                return _InvoiceNumber;
            }
            set
            {
                OnInvoiceNumberChanging(value);
                ReportPropertyChanging("InvoiceNumber");
                _InvoiceNumber = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("InvoiceNumber");
                OnInvoiceNumberChanged();
            }
        }
        private global::System.String _InvoiceNumber;
        partial void OnInvoiceNumberChanging(global::System.String value);
        partial void OnInvoiceNumberChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String TransactionID
        {
            get
            {
                return _TransactionID;
            }
            set
            {
                OnTransactionIDChanging(value);
                ReportPropertyChanging("TransactionID");
                _TransactionID = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("TransactionID");
                OnTransactionIDChanged();
            }
        }
        private global::System.String _TransactionID;
        partial void OnTransactionIDChanging(global::System.String value);
        partial void OnTransactionIDChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.String Message
        {
            get
            {
                return _Message;
            }
            set
            {
                OnMessageChanging(value);
                ReportPropertyChanging("Message");
                _Message = StructuralObject.SetValidValue(value, false);
                ReportPropertyChanged("Message");
                OnMessageChanged();
            }
        }
        private global::System.String _Message;
        partial void OnMessageChanging(global::System.String value);
        partial void OnMessageChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Decimal Amount
        {
            get
            {
                return _Amount;
            }
            set
            {
                OnAmountChanging(value);
                ReportPropertyChanging("Amount");
                _Amount = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Amount");
                OnAmountChanged();
            }
        }
        private global::System.Decimal _Amount;
        partial void OnAmountChanging(global::System.Decimal value);
        partial void OnAmountChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Boolean Approved
        {
            get
            {
                return _Approved;
            }
            set
            {
                OnApprovedChanging(value);
                ReportPropertyChanging("Approved");
                _Approved = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("Approved");
                OnApprovedChanged();
            }
        }
        private global::System.Boolean _Approved;
        partial void OnApprovedChanging(global::System.Boolean value);
        partial void OnApprovedChanged();

        #endregion

    
    }
    
    /// <summary>
    /// No Metadata Documentation available.
    /// </summary>
    [EdmEntityTypeAttribute(NamespaceName="TWOModel", Name="OrderDetail")]
    [Serializable()]
    [DataContractAttribute(IsReference=true)]
    public partial class OrderDetail : EntityObject
    {
        #region Factory Method
    
        /// <summary>
        /// Create a new OrderDetail object.
        /// </summary>
        /// <param name="id">Initial value of the Id property.</param>
        public static OrderDetail CreateOrderDetail(global::System.Int32 id)
        {
            OrderDetail orderDetail = new OrderDetail();
            orderDetail.Id = id;
            return orderDetail;
        }

        #endregion

        #region Primitive Properties
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=true, IsNullable=false)]
        [DataMemberAttribute()]
        public global::System.Int32 Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id != value)
                {
                    OnIdChanging(value);
                    ReportPropertyChanging("Id");
                    _Id = StructuralObject.SetValidValue(value);
                    ReportPropertyChanged("Id");
                    OnIdChanged();
                }
            }
        }
        private global::System.Int32 _Id;
        partial void OnIdChanging(global::System.Int32 value);
        partial void OnIdChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.DateTime> OrdDate
        {
            get
            {
                return _OrdDate;
            }
            set
            {
                OnOrdDateChanging(value);
                ReportPropertyChanging("OrdDate");
                _OrdDate = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("OrdDate");
                OnOrdDateChanged();
            }
        }
        private Nullable<global::System.DateTime> _OrdDate;
        partial void OnOrdDateChanging(Nullable<global::System.DateTime> value);
        partial void OnOrdDateChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Status
        {
            get
            {
                return _Status;
            }
            set
            {
                OnStatusChanging(value);
                ReportPropertyChanging("Status");
                _Status = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Status");
                OnStatusChanged();
            }
        }
        private global::System.String _Status;
        partial void OnStatusChanging(global::System.String value);
        partial void OnStatusChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String FormData
        {
            get
            {
                return _FormData;
            }
            set
            {
                OnFormDataChanging(value);
                ReportPropertyChanging("FormData");
                _FormData = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("FormData");
                OnFormDataChanged();
            }
        }
        private global::System.String _FormData;
        partial void OnFormDataChanging(global::System.String value);
        partial void OnFormDataChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String OrdNo
        {
            get
            {
                return _OrdNo;
            }
            set
            {
                OnOrdNoChanging(value);
                ReportPropertyChanging("OrdNo");
                _OrdNo = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("OrdNo");
                OnOrdNoChanged();
            }
        }
        private global::System.String _OrdNo;
        partial void OnOrdNoChanging(global::System.String value);
        partial void OnOrdNoChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String Operator
        {
            get
            {
                return _Operator;
            }
            set
            {
                OnOperatorChanging(value);
                ReportPropertyChanging("Operator");
                _Operator = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("Operator");
                OnOperatorChanged();
            }
        }
        private global::System.String _Operator;
        partial void OnOperatorChanging(global::System.String value);
        partial void OnOperatorChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public global::System.String CustomerName
        {
            get
            {
                return _CustomerName;
            }
            set
            {
                OnCustomerNameChanging(value);
                ReportPropertyChanging("CustomerName");
                _CustomerName = StructuralObject.SetValidValue(value, true);
                ReportPropertyChanged("CustomerName");
                OnCustomerNameChanged();
            }
        }
        private global::System.String _CustomerName;
        partial void OnCustomerNameChanging(global::System.String value);
        partial void OnCustomerNameChanged();
    
        /// <summary>
        /// No Metadata Documentation available.
        /// </summary>
        [EdmScalarPropertyAttribute(EntityKeyProperty=false, IsNullable=true)]
        [DataMemberAttribute()]
        public Nullable<global::System.Decimal> OrdTotal
        {
            get
            {
                return _OrdTotal;
            }
            set
            {
                OnOrdTotalChanging(value);
                ReportPropertyChanging("OrdTotal");
                _OrdTotal = StructuralObject.SetValidValue(value);
                ReportPropertyChanged("OrdTotal");
                OnOrdTotalChanged();
            }
        }
        private Nullable<global::System.Decimal> _OrdTotal;
        partial void OnOrdTotalChanging(Nullable<global::System.Decimal> value);
        partial void OnOrdTotalChanged();

        #endregion

    
    }

    #endregion

    
}
