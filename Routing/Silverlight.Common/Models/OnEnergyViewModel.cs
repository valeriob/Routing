using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;

using ReactiveUI;
using Silverlight.Common.Entities;


namespace Silverlight.Common.Models
{
    public class GlobalViewModel : ReactiveObject
    {
        #region Navigation

        private MenuItem _SelectedFunction;
        public MenuItem SelectedFunction 
        {
            get
            {
                if (_SelectedFunction == null)
                    return new MenuItem("Home", "Home", "", "/Images/Generics/Desktop.png",0);
                else
                    return _SelectedFunction;
            }
            set
            {
                _SelectedFunction = value;
                this.RaisePropertyChanged(v => v.SelectedFunction);
            }
        }

        #endregion
        
        public static string ResourceKey = "OnEnergyViewModel";

        public static Dictionary<string, object> Session;

        protected static GlobalViewModel _instance;
        public static GlobalViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GlobalViewModel();
                }
                return _instance;
            }
        }

        public GlobalViewModel()
        {
            Session = new Dictionary<string, object>();
        }
        
        protected bool _isBusy;
        public bool IsBusy { get { return _isBusy; } set { _isBusy = value; this.RaisePropertyChanged(v => v.IsBusy); } }

        protected Boolean _IsLoggedIn;
        public Boolean IsLoggedIn
        {
            get
            {
                return _IsLoggedIn;
            }
            set
            {
                _IsLoggedIn = value;
                if (!value)
                {
                    UserDescription = "";
                    UserId = -1;
                    MenuFunctions.Clear();
                    Modules.Clear();
                }
                this.RaisePropertyChanged(v => v.IsLoggedIn);
            }
        }

        protected string _serviceUrl;
        public string ServiceUrl { get { return _serviceUrl; } set { _serviceUrl = value; this.RaisePropertyChanged(v => v.ServiceUrl); } }

        protected int installationId;
        public int InstallationId { get { return installationId; } set { installationId = value; this.RaisePropertyChanged(v => v.InstallationId); } }

        protected String installationName;
        public String InstallationName
        {
            get
            {
                //if (String.IsNullOrEmpty(holdingName))
                //{
                //    return "Nessuno selezionato";
                //}
                return installationName;
            }
            set
            {
                installationName = value; 
                this.RaisePropertyChanged(v => v.InstallationName);
            }
        }

        //protected int? _codiceAzienda;
        //public int? CodiceAzienda { get { return _codiceAzienda; } set { _codiceAzienda = value; this.RaisePropertyChanged(v => v.CodiceAzienda); } }

        protected string _urlOnPortal;
        public string UrlOnPortal { get { return _urlOnPortal; } set { _urlOnPortal = value; this.RaisePropertyChanged(v => v.UrlOnPortal); } }

        protected string applicationName;
        public string ApplicationName { get { return applicationName; } set { applicationName = value; this.RaisePropertyChanged(v => v.ApplicationName); } }

        protected int? userId;
        public int? UserId
        { 
            get {
                return userId; 
            } 
            set {
                userId = value;
                this.RaisePropertyChanged(v => v.UserId);
            } 
        }

        protected String userDescription;
        public String UserDescription
        {
            get
            {
                return userDescription;
            }
            set
            {
                userDescription = value;
                this.RaisePropertyChanged(v => v.UserDescription);
            }
        }

        private ObservableCollection<MenuItem> _menuFunctions;
        public ObservableCollection<MenuItem> MenuFunctions
        {
            get { return _menuFunctions ?? ( _menuFunctions = new ObservableCollection<MenuItem>()); }
            set
            {
                _menuFunctions = value;
                this.RaisePropertyChanged(v => v.MenuFunctions);
            }
        }

        private ObservableCollection<MenuItem> modules;
        public ObservableCollection<MenuItem> Modules
        {
            get { return modules ?? (modules = new ObservableCollection<MenuItem>()); }
            set
            {
                modules = value;
                this.RaisePropertyChanged(v => v.Modules);
            }
        }
        //private ObservableCollection<Installation> _installations;
        //public ObservableCollection<Installation> Installations
        //{
        //    get { return _installations ?? (_installations = new ObservableCollection<Installation>()); }
        //    set
        //    {
        //        _installations = value;
        //        this.RaisePropertyChanged(v => v.Installations);
        //    }
        //}

        //private Installation _selectedInstallation;
        //public Installation SelectedInstallation
        //{
        //    get
        //    {
        //        return _selectedInstallation;
        //    }
        //    set
        //    {
        //        _selectedInstallation = value;
        //        OnEnergyViewModel.Instance.InstallationId = value.Id;
        //        OnEnergyViewModel.Instance.InstallationName = value.Name;
        //        IsInstallationEnabled = !(value.Id > 0);
        //        this.RaisePropertyChanged(v => v.SelectedInstallation);
        //    }
        //}

        //private bool isInstallationEnabled;
        //public bool IsInstallationEnabled
        //{
        //    get 
        //    {
        //        return isInstallationEnabled;
        //    }
        //    set
        //    {
        //        isInstallationEnabled = value;
        //        this.RaisePropertyChanged(v => v.IsInstallationEnabled);
        //    }
        //}
	
        //private ObservableCollection<FunctionItem> _menuPreferiti;
        //public ObservableCollection<FunctionItem> MenuPreferiti
        //{
        //    get { return _menuPreferiti ?? ( _menuPreferiti = new ObservableCollection<FunctionItem>()); }
        //    set { _menuPreferiti = value; this.RaisePropertyChanged(v => v.MenuFunctions); }
        //}

        //private string _nomeApplicativo;
        //public string NomeApplicativo
        //{
        //    get { return _nomeApplicativo; }
        //    set
        //    {
        //        _nomeApplicativo = value;
        //        this.RaisePropertyChanged(v => v.NomeApplicativo);
        //    }
        //}
        
        //private ConfigurationParametersDto _ApplicationParameters;
        //public ConfigurationParametersDto ApplicationParameters
        //{
        //    get { return _ApplicationParameters; }
        //    set
        //    {
        //        _ApplicationParameters = value;
        //        this.RaisePropertyChanged(v => v.ApplicationParameters);
        //    }
        //}

        public void Refresh() 
        {
        }

        #region ping
        //DispatcherTimer PingTimer ;
        //ServiceAuthenticationV2.ServiceAuthenticationV2Client authService;

        //private void StartPing()
        //{
            
        //    PingTimer= new DispatcherTimer();
        //    authService = new ServiceAuthenticationV2.ServiceAuthenticationV2Client();
        //    PingTimer.Tick += new EventHandler(PingTimer_Tick);

        //    // startup
        //    authService.PingCompleted+=(sender,e)=>{
        //        if (e.Error==null){
        //            bool? isStartup = e.UserState as bool?;
        //            if (isStartup.HasValue && isStartup.Value){
        //                PingTimer.Interval = e.Result;
        //                PingTimer.Start();
        //            }
        //        }
        //    };

        //    authService.PingAsync(Application, true);
        //}

        //void PingTimer_Tick(object sender, EventArgs e)
        //{
        //    authService.PingAsync(Application, false);
        //}
        #endregion

        //public void RefreshPreferiti(ObservableCollection<ServiceAuthenticationV2.MenuFunc> lstItem)
        //{
        //    //ABA Impostazione Preferiti
        //    foreach (var item in OnEnergyViewModel.Instance.MenuFunctions)
        //    {
        //        foreach (var item2 in item.Children)
        //        {
        //            foreach (var item3 in item2.Children)
        //            {
        //                if ((from p in lstItem where p.Codice == item3.Id select p).Any())
        //                    item3.IsPreferito = true;    
        //            }
        //        }
        //    }


        //    OnEnergyViewModel.Instance.MenuPreferiti.Clear();
        //    var collezionePreferiti = new ObservableCollection<FunctionItem>();
        //    foreach (var item in OnEnergyViewModel.Instance.MenuFunctions)
        //    {
        //        FunctionItem functionItem = new FunctionItem(item.Id, item.Description, item.Function);
        //        functionItem.Children = new ObservableCollection<FunctionItem>();
        //        foreach (var categoria in item.Children)
        //        {
        //            FunctionItem functionItemCat = new FunctionItem(categoria.Id, categoria.Description, categoria.Function, categoria.Image, categoria.Color,categoria.Posizione,false);
        //            functionItemCat.Children = new ObservableCollection<FunctionItem>();
        //            var preferiti = from a in categoria.Children
        //                            join b in lstItem on a.Id equals b.Codice
        //                            select a;
        //            foreach (var funzione in preferiti)
        //            {
        //                functionItemCat.Children.Add(new FunctionItem(funzione.Id, funzione.Description, funzione.Function, funzione.Image, funzione.Color,funzione.Posizione,false));
        //            }
        //            functionItem.Children.Add(functionItemCat);
        //        }
        //        collezionePreferiti.Add(functionItem);
        //    }
        //    OnEnergyViewModel.Instance.MenuPreferiti = collezionePreferiti;
        //}
    
    }

}
