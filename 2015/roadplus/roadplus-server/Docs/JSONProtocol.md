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
    "type": 0,
    "activity-type": 0,
    "message": "info/error",
    "payload": {
        "object": { object }
    }
}
```

Server will always send a response in this format.

type = member of response enum

activity-type = if the response type was acknoledge, this contains a
member of the activity enum (to indicate what action was successful)

message = a user-friendly description of the response

payload = (always an array!) if the type is acknoledge and the activity 
has a product, the product is broadcasted to every client


## Message structure
```json
{
    "type": 0,
    "payload": {
        "object": { object }
    }
}
```

type = activity type (see enum)

payload = (always an array!) an object as specified below


## Payload and activity types
### Identify
```json
{
    "type": 1,
    "payload": {
        "LinkType": <member of link enum>
    }
}
```

Expected response:
```json
{
    "type": 0,
    "activity-type": 1,
    "message": "Identification successful",
    "payload": null
}
```


### Get
#### Ports
```json
{
    "type": 2,
    "payload": {
        "System.String": "ports"
    }
}
```

Expected response:
```json
{
    "type": 0,
    "activity-type": 2,
    "message": "",
    "payload": {[
        "port1",
        "port2",
        "etc..."
    ]}
}
```

#### All entities
```json
{
    "type": 2,
    "payload": {
        "System.String": "all"
    }
}
```

Expected response:
```json
{
    "type": 0,
    "activity-type": 2,
    "message": "",
    "payload": {
        "zones": [
            { <zone1> },
            { <zone2> },
            { <etc...> },
        ],
        "schools": [
            { <school1> },
            { <school2> },
            { <etc...> },
        ],
        "roadconstructions": [
            { <rc1> },
            { <rc2> },
            { <etc...> },
        ],
        
    }
}
```

#### Zone
```json
{
    "type": 2,
    "payload": {
        "Zone": "all" or <id>
    }
}
```

Expected response:
```json
{
    "type": 0,
    "activity-type": 2,
    "message": "",
    "payload": {
        "zones": [
            "zone": { <zone1> },
            "zone": { <zone2> },
            "zone": { <etc...> },
        ]
    }
}
```

#### School
```json
{
    "type": 2,
    "payload": {
        "School": "all" or <id>
    }
}
```

Expected response:
```json
{
    "type": 0,
    "activity-type": 2,
    "message": "",
    "payload": {
        "schools": [
            "School": { <school1> },
            "School": { <school2> },
            "School": { <etc...> },
        ]
    }
}
```


#### RoadConstruction
```json
{
    "type": 2,
    "payload": {
        "RoadConstruction": "all" or <id>
    }
}
```

Expected response:
```json
{
    "type": 0,
    "activity-type": 2,
    "message": "",
    "payload": [
        "roadconstructions": [
            "RoadConstruction": { <rc1> },
            "RoadConstruction": { <rc2> },
            "RoadConstruction": { <etc...> },
        ]
    ]
}
```

### Set
```json
{
    "type": 3,
    "payload": [
        "<target>": {
            "id": <id>,
            "property": "value", etc...
        }
    ]
}
```

Expected response:
```json
{
    "type": 0,
    "activity-type": 3,
    "message": "",
    "payload": [
        "<target>": {
            "id": <id>,
            "property": "value", etc...
        }
    ]
}
```

Where &lt;target&gt; = object type to act on

### Create
```json
{
    "type": 4,
    "payload": {
        "<target>": { <object> },
        etc...
    }
}
```

Expected response:
```json
{
    "type": 0,
    "activity-type": 4,
    "message": "",
    "payload": {
        "<target>": { object }, etc...
    }
}
```

Where &lt;target&gt; = the object type to create and &lt;object&gt; = 
the object data as specified below

#### Create objects
Zone
```json
{
        "name": "Noord",
        "x": <x>,
        "y": <y>,
}
```

School
```json
{
        "name": "Fontys",
        "dateStart": "hh-mm",
        "dateEnd": "hh-mm",
}
```
Road Construction
```json
{
        "name": "Road Construction",
        "dateStart": "dd-mm-yyyy",
        "dateEnd": "dd-mm-yyyy",
}
```


### Remove
```json
{
    "type": 5,
    "payload": [
        "<target>": <id>, etc...
    ]
}
```

Expected response:
```json
{
    "type": 0,
    "activity-type": 5,
    "message": "",
    "payload": [
        "<target>": <id>
    ]
}
```

Where &lt;target&gt; = the kind of object to remove and &lt;id&gt; is 
the id to remove

### Disconnect
not needed - use socket.close() or someting

