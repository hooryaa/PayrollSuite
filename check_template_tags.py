from pathlib import Path
path = Path(r'C:\Users\user\.nuget\packages\devexpress.blazor\25.1.3\lib\net8.0\DevExpress.Blazor.v25.1.dll')
if not path.exists():
    raise FileNotFoundError(path)
info = {}
for token in [b'DisplayTemplate', b'CellDisplayTemplate', b'GridCommandColumnCellTemplate', b'DataItemTemplate', b'ItemTemplate', b'BodyTemplate', b'HeaderTemplate']:
    info[token.decode('ascii')] = token in path.read_bytes()
for k,v in info.items():
    print(k, v)
