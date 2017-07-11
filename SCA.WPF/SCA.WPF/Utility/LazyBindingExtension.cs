using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Markup;
using System.Windows;
using System.Windows.Data;
/* ==============================
*
* Author     : William
* Create Date: 2017/7/8 12:13:25
* FileName   : LazyBindingExtension
* Description:
* Version：V1
* ===============================
*/
namespace SCA.WPF.Utility
{
[MarkupExtensionReturnType(typeof(object))]
public class LazyBindingExtension : MarkupExtension
{
    public LazyBindingExtension()
    { }

    public LazyBindingExtension(string path)
    {
        Path = new PropertyPath(path);
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        IProvideValueTarget service = serviceProvider.GetService
            (typeof(IProvideValueTarget)) as IProvideValueTarget;
        if (service == null)
            return null;

        mTarget = service.TargetObject as FrameworkElement;
        mProperty = service.TargetProperty as DependencyProperty;
        if (mTarget != null && mProperty != null)
        {
            mTarget.IsVisibleChanged += OnIsVisibleChanged;
            return null;
        }
        else
        {
            Binding binding = CreateBinding();
            return binding.ProvideValue(serviceProvider);
        }
    }

    private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
    {
        Binding binding = CreateBinding();
        BindingOperations.SetBinding(mTarget, mProperty, binding);
    }

    private Binding CreateBinding()
    {
        Binding binding = new Binding(Path.Path);
        if (Source != null)
            binding.Source = Source;
        if (RelativeSource != null)
            binding.RelativeSource = RelativeSource;
        if (ElementName != null)
            binding.ElementName = ElementName;
        binding.Converter = Converter;
        binding.ConverterParameter = ConverterParameter;
        return binding;
    }

    #region Fields
    private FrameworkElement mTarget = null;
    private DependencyProperty mProperty = null;
    #endregion

    #region Properties
    public object Source{get;set;}
    public RelativeSource RelativeSource{get;set;}
    public string ElementName{get;set;}
    public PropertyPath Path{get;set;}
    public IValueConverter Converter{get;set;}
    public object ConverterParameter { get; set; }
    #endregion
}
}
