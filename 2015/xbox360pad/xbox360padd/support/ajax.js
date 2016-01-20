window.onload = function()
{
    window.setInterval(updatePadInfo, 500);
    updatePadInfo();
}

function setLeds()
{
    var mode = document.getElementById("ledmodes").value;
    var request = new XMLHttpRequest();
    request.open("GET", "xbox360padremote?setleds=" + mode, true);
    request.send();
}

function setRumble()
{
    var bigw = document.getElementById("bigw").value;
    var smallw = document.getElementById("smallw").value;

    if (bigw > 255 ||
        smallw > 255)
    {
        alert("Big weight and small weight must both be smaller or equal to 255");
        return;
    }

    var request = new XMLHttpRequest();
    request.open("GET", "xbox360padremote?setrumble=" + bigw + "," + smallw, true);
    request.send();
}

function updatePadInfo()
{
    var request = new XMLHttpRequest();
    request.onreadystatechange = function() {
      if (request.readyState === 4 && request.status === 200) {
        parseJson(request.responseText);
      }
    };
    request.open("GET", "xbox360padremote?update", true);
    request.send();
}

function parseJson(json)
{
    var state = JSON.parse(json);

    for (var button in state.buttons)
    {
        var buttontable = document.getElementById("buttons");
        var dpadtable = document.getElementById("dpad");

        try
        {
            buttontable.getElementsByClassName(button)[0].innerHTML
                    = button + ": " + state.buttons[button];
        }
        catch (TypeError)
        {
            dpadtable.getElementsByClassName(button)[0].innerHTML
                    = button + ": " + state.buttons[button];
        }
    }

    for (var stick in state.sticks)
    {
        var stickstable = document.getElementById("sticks");
        stickstable.getElementsByClassName(stick)[0].innerHTML
                = stick + ": " + state.sticks[stick];
    }

    for (var trigger in state.triggers)
    {
        var triggertable = document.getElementById("triggers");
        triggertable.getElementsByClassName(trigger)[0].innerHTML
                = trigger + ": " + state.triggers[trigger];
    }
}
