from pathlib import Path
import re

path = Path(r'C:\Users\user\.nuget\packages\devexpress.blazor\25.1.3\lib\net8.0\DevExpress.Blazor.v25.1.dll')
if not path.exists():
    raise FileNotFoundError(path)

data = path.read_bytes()

print('Component support present in assembly:')
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
    b'DxLoadingPanel',
    b'DxButton',
    b'DxValidationSummary',
    b'NotificationType',
    b'DxValidator',
    b'DxGridCommandColumn',
    b'CellDisplayTemplate',
    b'DisplayTemplate',
    b'ToolbarItem',
    b'EditorTemplate'
]:
    print(f'{token.decode("ascii")}:', token in data)

print('\nMore matching names:')
strings = re.findall(rb'[A-Za-z0-9_]{5,120}', data)
for prefix in [b'DxGrid', b'DxPopup', b'DxToolbar', b'DxButton', b'DxFormLayout', b'DxDateEdit', b'DxComboBox', b'DxCheckBox', b'DxSpinEdit', b'DxNotification']:
    hits = sorted({s.decode('utf-8', errors='ignore') for s in strings if s.startswith(prefix)})
    if hits:
        print('---', prefix.decode('utf-8'), '---')
        for hit in hits[:100]:
            print(hit)

print('\nTemplate-like strings:')
template_hits = sorted({s.decode('utf-8', errors='ignore') for s in strings if b'Template' in s})
for hit in template_hits[:200]:
    print(hit)
print('Total template-like strings:', len(template_hits))
