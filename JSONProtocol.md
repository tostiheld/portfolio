JSON Messages 
=================
## Objects
### Zone
```json
{
		'id': '0',
        'name': 'Noord',
        'x': 'hh-mm',
        'y': 'hh-mm',
}
```

### School
```json
{
		'id': '0',
        'name': 'Fontys',
        'dateStart': 'hh-mm',
        'dateEnd': 'hh-mm',
}
```
### Road Construction
```json
{
		'id': '0',
        'name': 'Road Construction',
        'dateStart': 'dd-mm-yyyy',
        'dateEnd': 'dd-mm-yyyy',
}
```
### Port List
```json
{
	'port1',
    'port2',
    ...
}
```

## Action Types
* 0: Add
* 1: Remove
* 2: Edit
* 3: Error
* 4: Identify

## Payload Types
* 0: Zone
* 1: School
* 2: RoadConstruction
* 3: PortList

## GUI -> Server
### Identification
This is send on connection
```json
{
    'action': '4',
    'payload': 'UI',
    'payloadtype': ''
}
```

### Disconnect
This is send on connection
```json
{
    'action': '4',
    'payload': 'Disconnect',
    'payloadtype': ''
}
```

### New School/RoadConstruction/Zone
```json
{
    'action': '0',
    'payload': <object>,
    'payloadtype': 'payloadType'
}
```
### Remove School/RoadConstruction/Zone
```json
{
    'action': '1',
    'payload': {
		'id': 'object.id'
    },
    'payloadtype': 'payloadType'
}
```
### Edit School/RoadConstruction/Zone
```json
{
    'action': '2',
    'payload': <object>,
    'payloadtype': 'payloadType'
}
```
## Server -> GUI
### New School/RoadConstruction/Zone
```json
{
    'action': '0',
    'payload': <object>,
    'payloadtype': 'payloadType'
}
```
### Remove School/RoadConstruction/Zone
```json
{
    'action': '1',
    'payload': {
		'id': 'object.id'
    },
    'payloadtype': 'payloadType'
}
```
### Edit School/RoadConstruction/Zone
```json
{
    'action': '2',
    'payload': <object>,
    'payloadtype': 'payloadType'
}
```
### Action Failed:
```json
{
    'action': '3',
    'payload': 'errorMessage',
    'payloadtype': 'payloadType'
}
```