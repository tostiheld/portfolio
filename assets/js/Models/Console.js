/**
 * The console outputs data to the screen
 */
function Console(consoleElement) {
    this.con = consoleElement;

    this.appendFromClient = function (data) {
        this.append('<div class="msg"><i class="icon-laptop">You said:</i><pre>' + this.encodeHTML(data) + "</pre></div>");
    }
    this.appendFromServer = function (data) {
        this.append('<div class="msg"><i class="icon-white icon-cloud">Server said:</i><pre>' + this.encodeHTML(data) + "</pre></div>");
    }
    this.appendError = function (data) {
        this.append('<div class="msg"><i class="icon-white icon-warning-sign">Error:</i><pre>' + this.encodeHTML(data) + "</pre></div>");
    }

    this.append = function (data) {
        // only scroll to bottom if we are currently 
        // scrolled down
        // if the user is browsing earlier content, 
        // don't force them down
        // the page when the console updates
        scrollToBottom = false
        if (this.isAtBottom()) {
            scrollToBottom = true;
        }
        this.con.html(this.con.html() + data);
        if (scrollToBottom) this.scrollToBottom();
    };
    /**
     * scroll the console to the bottom
     */
    this.scrollToBottom = function () {
        this.con.scrollTop(this.con[0].scrollHeight);
    };
    /**
     * determine if the console is currently
     * scrolled to the bottom
     */
    this.isAtBottom = function () {
        return (this.con[0].scrollHeight - this.con[0].clientHeight <= this.con.scrollTop());
    }

    /**
     * encode HTML so it's safe for printing
     */
    this.encodeHTML = function (message) {
        // this method taken from:
        // http://stackoverflow.com/questions/1219860/html-encoding-in-javascript-jquery
        return $('<div/>').text(message).html();
    }

    this.getLocalCommands = function () {
        var LatestCommands;
        if (localStorage.commands === null || localStorage.commands == "" || typeof localStorage.commands == "undefined") {
            LatestCommands = [];
            console.log("no storage set");
        } else {
            LatestCommands = JSON.parse(localStorage.commands);
        }
        return LatestCommands;
    }

    this.addToLatestCommands = function (message) {
        message = $.trim(message);
        var LatestCommands = this.getLocalCommands();
        for (var key in LatestCommands) {
            if (LatestCommands[key] == message) {
                LatestCommands.splice(key, 1);
            }
        }
        LatestCommands.push(message);
        localStorage.commands = JSON.stringify(LatestCommands);
    }

    //Handels key events for the console
    //e = eventData
    //element = the consoleInput element
    this.handleKeyEvents = function (e, element) {
        console.log("test");
        if (e.which == 13) {
            //enter
            $("#send").trigger("click");
        } else if (e.which == 38) {
            //omhoog
            LatestCommands = this.getLocalCommands();
            dl(LatestCommands);
            if (typeof window.commandIndex == "undefined" || window.commandIndex < 0) {
                window.commandIndex = LatestCommands.length;
            } else {
                window.commandIndex--;
            }
            if (window.commandIndex == 0 || window.commandIndex > LatestCommands.length) {
                $(element).val("");
                window.commandIndex--;

            } else {
                $(element).val(LatestCommands[window.commandIndex - 1]);
            }
            dl(window.commandIndex);

        } else if (e.which == 40) {
            //omlaag
            LatestCommands = this.getLocalCommands();
            dl(LatestCommands);
            if (typeof window.commandIndex == "undefined" || window.commandIndex >= LatestCommands.length || window.commandIndex < 0) {
                $(element).val("");
                window.commandIndex = -1;

            } else {
                window.commandIndex++;
                $(element).val(LatestCommands[window.commandIndex - 1]);
            }
            dl(window.commandIndex);
        }
    }
}