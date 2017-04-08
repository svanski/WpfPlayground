using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using WpfTutorials.Properties;

namespace WpfTutorials.UI.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged, INotifyDataErrorInfo
    {

        private readonly IDictionary<string, PropertyValidatoinInfo> _ruleMap = new Dictionary<string, PropertyValidatoinInfo>();

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
        protected void SetValue<T>(Expression<Func<T>> expression, ref T storage, T value)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value))
                return;

            storage = value;
            OnPropertyChanged(expression);
        }

        protected void SetValueWithoutCompare<T>(Expression<Func<T>> expression, ref T storage, T value)
        {
            storage = value;
            OnPropertyChanged(expression);
        }

        protected virtual void OnPropertyChanged<T>(Expression<Func<T>> expression)
        {
            var property = GetPropertyInfo(expression);
            OnPropertyChanged(property.Name);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #region DataErrorInfo

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public string this[string columnName]
        {
            get
            {
                if (_ruleMap.ContainsKey(columnName))
                {
                    var propValidationInfo = _ruleMap[columnName];
                    propValidationInfo.Validate();
                    return propValidationInfo.ErrorMessage;
                }
                return string.Empty;
            }
        }

        public bool HasErrors
        {
            get
            {
                if (_ruleMap.Count == 0)
                    return false;

                var hasError = false;
                _ruleMap.Values.Any(validator =>
                {
                    validator.Validate();
                    hasError = validator.HasError;
                    return hasError;
                });

                return hasError;
            }
        }

        public string Error { get; set; }

        #endregion

        #region Helper Functions
        protected void AddRule(string propertyName, Func<bool> ruleDelegate, string errorMessage)
        {
            _ruleMap.Add(propertyName, new PropertyValidatoinInfo(ruleDelegate, errorMessage));
        }

        protected PropertyInfo GetPropertyInfo<T>(Expression<Func<T>> propertyExpression)
        {
            var memberExpression = propertyExpression.Body as MemberExpression;
            if (memberExpression == null)
                throw new ArgumentException(Resources.PropertySupport_NotMemberAccessExpression_Exception);

            var propertyInfo = memberExpression.Member as PropertyInfo;
            if (propertyInfo == null)
                throw new ArgumentException(Resources.PropertySupport_ExpressionNotProperty_Exception);

            var isExpressionValid = propertyInfo != null &&
                                    propertyInfo.CanRead &&
                                    !propertyInfo.GetMethod.IsPrivate;

            if (!isExpressionValid)
            {
                throw new InvalidOperationException(Resources.Invalid_Lambda_Expression);
            }

            return propertyInfo;
        }

        public IEnumerable GetErrors(string propertyName)
        {
            throw new NotImplementedException();
        }
        #endregion
    }

    internal class PropertyValidatoinInfo
    {
        private readonly Func<bool> _validationRun;
        private readonly string _errorMessage;

        public PropertyValidatoinInfo(Func<bool> validationRun, string errorMessage)
        {
            _validationRun = validationRun;
            _errorMessage = errorMessage;
        }

        public string ErrorMessage { get; private set; }

        public bool HasError { get; private set; }

        public void Validate()
        {
            try
            {
                HasError = !_validationRun();
                ErrorMessage = HasError ? _errorMessage : string.Empty;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                HasError = true;
            }
        }
    }
}
