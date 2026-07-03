from pathlib import Path
import re

path = Path(r'C:\Users\user\.nuget\packages\devexpress.blazor\25.1.3\lib\net8.0\DevExpress.Blazor.v25.1.dll')
if not path.exists():
    raise FileNotFoundError(path)

b = path.read_bytes()
text = ''.join(chr(ch) if 32 <= ch < 127 else ' ' for ch in b)
for prefix in ['Dx', 'Display', 'Notification', 'Toast', 'Card', 'Panel', 'Popup', 'Dialog', 'Grid', 'Button', 'Form', 'Validator', 'Toolbar', 'Loading', 'Spin', 'Combo']:
    print('---', prefix)
    found = set(re.findall(r'\b' + re.escape(prefix) + r'[A-Za-z0-9_]+\b', text))
    names = sorted([name for name in found if len(name) <= 40])
    print(' '.join(names))
