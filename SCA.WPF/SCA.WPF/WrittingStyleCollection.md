{Binding RelativeSource={RelativeSource Self},  Path=(Validation.Errors)[0].ErrorContent}

*　行验证
```
<local:DetailInfoGridControl.RowValidationRules>
    <validation:DeviceInfo8007Rule ValidationStep="UpdatedValue"/>                    
</local:DetailInfoGridControl.RowValidationRules>
```
* 如何写一个RowStyle?????????  以下代码有问题
                <local:DetailInfoGridControl.RowStyle>
                    <Style  TargetType="{x:Type DataGridRow}">
                        <Setter Property="SnapsToDevicePixels" Value="true"/>
                        <Setter Property="Validation.ErrorTemplate" Value="{x:Null}"/>
                        <Setter Property="ValidationErrorTemplate">
                            <Setter.Value>
                                <ControlTemplate>
                                    <Grid>
                                        <Ellipse Width="12" Height="12" Margin="0 2 0 0"
                        Fill="Red" Stroke="Black" VerticalAlignment="Top"
                        StrokeThickness="0.5"/>
                                        <TextBlock FontWeight="Bold" Padding="4,0,0,0"
                        VerticalAlignment="Top" Foreground="White" Text="***"
                        ToolTip="{Binding (Validation.Errors)[0].ErrorContent, RelativeSource={RelativeSource FindAncestor, AncestorType={x:Type DataGridRow}}}"/>
                                    </Grid>
                                </ControlTemplate>
                            </Setter.Value>
                        </Setter>
                    </Style>

                </local:DetailInfoGridControl.RowStyle>

* 使用ToolTipService的占位符的方式，设置ToolTip(未起作用)
     <ControlTemplate>
        <TextBlock Margin="2,0,0,0" VerticalAlignment="Center"
    HorizontalAlignment="Center"
    TextAlignment="Center"
    Foreground="Red" Text="**"
    ToolTipService.PlacementTarget="{Binding  RelativeSource={x:Static RelativeSource.Self}}"
    >                
    <TextBlock.ToolTip >
        <ToolTip>
            <TextBlock Text="{Binding  Path=(Validation.Errors)[0].ErrorContent}"/>                       
        </ToolTip>
    </TextBlock.ToolTip>
      </TextBlock>
    </ControlTemplate>