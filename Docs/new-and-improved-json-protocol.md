# The real, new and improved JSON Message Protocol

## Chapter Zero: Error response

An error is always reported as follows:
```json
{
    "response": "error",
    "error-message": "<generated_by_server>",
    "id": <received_command_id>
}
```

## Chapter One: Create commands

This chapter is all about commands to create objects on the 
management server.

### Creation success

If an object creation is successful, the following response 
is expected:
```json
{
    "response": "OK",
    "id": <received_command_id>,
    "createdObject": { <object_as_created_by_server> }
}
```

### Object One: To create a Zone

The syntax to create a zone is as follows:
```json
{
    "id": <generated_id>,
    "command": "createZone",
    "name": "<preferred_zone_name>"
}
```

### Object Two: To create a Vertex

The syntax to create a vertex is as follows:
```json
{
    "id": <generated_id>,
    "command": "createVertex",
    "zoneId": <parent_zone>,
    "x": <preferred_x>,
    "y": <preferred_y>
}
```

### Object Three: To create an Edge

The syntax to create an edge is as follows:
```json
{
    "id": <generated_id>,
    "command": "createEdge",
    "zoneId": <parent_zone>,
    "startVertex": <start_id>,
    "endVertex": <end_id>
}
```

### Object Four:  To create a School

The syntax to create a school is as follows:
```json
{
    "id": <generated_id>,
    "command": "createSchool",
    "zoneId": <parent_zone>,
    "location": <vertex_id>,
    "openTime": "<preferred_open_time>",
    "closeTime": "<preferred_close_time>"
}
```

### Object Four: To create a Roadconstruction

The syntax to create a roadconstruction is as follows:
```json
{
    "id": <generated_id>,
    "command": "createRoadconstruction",
    "zoneId": <parent_zone>,
    "location": <edge_id>,
    "startDate": "<preferred_start_date>",
    "endDate": "<preferred_end_data>"
}
```

## Chapter Two: Remove commands

### Removal success

If a removal is successful, the following response is 
expected:
```json
{
    "response": "OK",
    "id": <received_command_id>,
    "removedObject": <removed_object_id>
}
```

### Object One: To remove a Zone

The syntax to remove a zone is as follows:
```json
{
    "id": <generated_id>,
    "command": "removeZone",
    "zoneId": <preferred_zone_id>
}
```

### Object Two: To remove a Vertex

The syntax to remove a vertex is as follows:
```json
{
    "id": <generated_id>,
    "command": "removeVertex",
    "vertexId": <preferred_vertex_id>
}
```

### Object Three: To remove an Edge

The syntax to remove an edge is as follows:
```json
{
    "id": <generated_id>,
    "command": "removeEdge",
    "edgeId": <preferred_edge_id>
}
```

### Object Four: To remove a School

The syntax to remove a school is as follows:
```json
{
    "id": <generated_id>,
    "command": "removeSchool",
    "schoolId": <preferred_school_id>
}
```

### Object One: To remove a Roadconstruction

The syntax to remove a roadconstruction is as follows:
```json
{
    "id": <generated_id>,
    "command": "removeRoadconstruction",
    "roadconstructionId": <preferred_roadconstruction_id>
}
```

## Chapter Three: Requesting objects

### Request success

If multiple objects were requested, the following 
response is expected:
```json
{
    "response": "OK",
    "id": <received_command_id>,
    "requestedObjects": [
        { <requested_object1> },
        { <requested_object2> },
        etc...
    ]
}
```

### Object One: To request Zones

The syntax to request zones is as follows:
```json
{
    "id": <generated_id>,
    "command": "requestZones"
}
```

### Object One: To request Vertices

The syntax to request vertices is as follows:
```json
{
    "id": <generated_id>,
    "command": "requestVertices"
}
```

### Object One: To request Edges

The syntax to request edges is as follows:
```json
{
    "id": <generated_id>,
    "command": "requestEdges"
}
```

### Object One: To request School

The syntax to request zones is as follows:
```json
{
    "id": <generated_id>,
    "command": "requestSchools"
}
```

### Object One: To request Roadconstructions

The syntax to request zones is as follows:
```json
{
    "id": <generated_id>,
    "command": "requestRoadconstructions"
}
```
