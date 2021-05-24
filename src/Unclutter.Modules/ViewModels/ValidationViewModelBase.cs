using Prism.Ioc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using Unclutter.SDK.IServices;

namespace Unclutter.Modules.ViewModels
{
    public abstract class ValidationViewModelBase : ViewModelBase, INotifyDataErrorInfo
    {
        /* Fields */
        private Dictionary<string, List<string>> _errors = new Dictionary<string, List<string>>();

        /* Properties */
        public bool IsValidating { get; set; } = true;
        public ILocalizationProvider LocalizationProvider { get; }

        protected ValidationViewModelBase() : base()
        {
            LocalizationProvider = ContainerLocator.Container.Resolve<ILocalizationProvider>();
        }

        /* Methods */
        protected override bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            var result = base.SetProperty(ref storage, value, propertyName);
            if (IsValidating) ValidateProperty(propertyName, value);
            return result;
        }
        protected override bool SetProperty<T>(ref T storage, T value, Action onChanged, [CallerMemberName] string propertyName = null)
        {
            var result = base.SetProperty(ref storage, value, onChanged, propertyName);
            if (IsValidating) ValidateProperty(propertyName, value);
            return result;
        }
        public void ClearValidationErrors()
        {
            _errors = new Dictionary<string, List<string>>();
            RaiseErrorsChanged(null);
            RaisePropertyChanged(null);
        }
        public void ClearValidationErrors(string propertyName)
        {
            _errors.Remove(propertyName);
            RaiseErrorsChanged(propertyName);
        }
        public bool TryGetErrors(string propertyName, out IEnumerable<string> errors)
        {
            try
            {
                errors = GetErrors(propertyName)?.Cast<string>();
                return errors != null && errors.Any();
            }
            catch (Exception)
            {
                errors = null;
                return false;
            }
        }

        /* Helpers */
        private void ValidateProperty<T>(string propertyName, T value)
        {
            var results = new List<ValidationResult>();
            var context = new ValidationContext(this) { MemberName = propertyName };
            context.InitializeServiceProvider(LocalizationServiceProvider);
            Validator.TryValidateProperty(value, context, results);

            if (results.Any())
            {
                _errors[propertyName] = results.Select(r => r.ErrorMessage).ToList();
            }
            else
            {
                _errors.Remove(propertyName);
            }
            RaiseErrorsChanged(propertyName);
        }

        private object LocalizationServiceProvider(Type arg)
        {
            return arg.Equals(typeof(ILocalizationProvider)) ? LocalizationProvider : null;
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        #region INotifyDataErrorInfo
        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors
        {
            get
            {
                return _errors.Any(propErrors => propErrors.Value != null && propErrors.Value.Count > 0);
            }
        }

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return null;

            return _errors.ContainsKey(propertyName) ? _errors[propertyName] : null;
        }
        #endregion
    }
}
