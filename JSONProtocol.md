JSON Messages 
=================
## Enums
### Activity enum
- 0: Unknown
- 1: Identify
- 2: Get
- 3: Set
- 4: Create
- 5: Remove

### LinkType enum
- 0: Unidentified
- 1: UI
- 2: Car
- 3: Road

### Response enum
- 0: Acknoledge
- 1: Information
- 2: Failure

## Response structure
```json
{
    'type': 0,
    'activity-type': 0,
    'message': 'info/error',
    'payload': {
        object
    }
    'payload-type': 'object'
}
```

Server will always send a response in this format.

type = member of response enum

activity-type = if the response type was acknoledge, this contains a
member of the activity enum (to indicate what action was successful)

message = a user-friendly description of the response

payload = if the type is acknoledge and the activity has a product, the 
product is broadcasted to every client

payload-type = type of the product in payload

## Message structure
```json
{
    'type': 0,
    'payload-type': 'object'
    'payload': {
        object
    }
}
```

type = activity type (see enum)

payload-type = what object to perform the activity on

payload = an object as specified below


## Payload and activity types
### Identify
```json
{
    'type': 1,
    'payload-type': 'LinkType'
    'payload': {
        <member of link enum>
    }
}
```

### Get
#### Ports
```json
{
    'type': 2,
    'payload-type': 'none'
    'payload': {
        "ports"
    }
}
```

#### Zone
```json
{
    'type': 2,
    'payload-type': 'Zone'
    'payload': {
        "all" or <id>
    }
}
```

#### School
```json
{
    'type': 2,
    'payload-type': 'School'
    'payload': {
        "all" or <id>
    }
}
```

#### RoadConstruction
```json
{
    'type': 2,
    'payload-type': 'RoadConstruction'
    'payload': {
        "all" or <id>
    }
}
```

### Set
```json
{
    'type': 3,
    'payload-type': '<target>'
    'payload': {
        "property": "value"
    }
}
```

Where &lt;target&gt; = object type to act on

### Create
```json
{
    'type': 4,
    'payload-type': '<target>'
    'payload': {
        <object>
    }
}
```

Where &lt;target&gt; = the object type to create and &lt;object&gt; = 
the object data as specified below

#### Create objects
Zone
```json
{
        'name': 'Noord',
        'x': 'hh-mm',
        'y': 'hh-mm',
}
```

School
```json
{
        'name': 'Fontys',
        'dateStart': 'hh-mm',
        'dateEnd': 'hh-mm',
}
```
Road Construction
```json
{
        'name': 'Road Construction',
        'dateStart': 'dd-mm-yyyy',
        'dateEnd': 'dd-mm-yyyy',
}
```


### Remove
```json
{
    'type': 5,
    'payload-type': '<target>'
    'payload': {
        <id>
    }
}
```

Where &lt;target&gt; = the kind of object to remove and &lt;id&gt; is 
the id to remove

### Disconnect
not needed - use socket.close() or someting

