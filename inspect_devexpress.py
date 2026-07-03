from pathlib import Path

path = Path(r'C:\Users\user\.nuget\packages\devexpress.blazor\25.1.3\lib\net8.0\DevExpress.Blazor.v25.1.dll')
if not path.exists():
    raise FileNotFoundError(path)

data = path.read_bytes()

for token in [
    b'DxCard',
    b'DxPopup',
    b'DxToolbar',
    b'DxNotification',
    b'DxGrid',
    b'DxFormLayout',
    b'DxTextBox',
    b'DxDateEdit',
    b'DxComboBox',
    b'DxCheckBox',
    b'DxSpinEdit',
    b'CardBodyTemplate',
    b'CardHeaderTemplate',
    b'DisplayTemplate',
    b'DxGridCommandColumnCellTemplate',
    b'DxToolbarItem',
    b'DxLoadingPanel',
    b'DxButton',
    b'DxDateEdit',
    b'DxComboBox',
    b'DxValidationSummary',
    b'NotificationType',
    b'DxCard',
    b'DxToolbar',
    b'DxValidator'
]:
    print(token.decode('ascii'), token in data)
