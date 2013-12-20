
function showProcessing() {
    var elem = document.getElementById('showProcessing');
    if (elem) {
        elem.height = window.height;
        elem.width = window.width;
        elem.className = 'processingAlertShow';
    }
    

}
function hideProcessing() {
    var elem = document.getElementById('showProcessing');
    if (elem) {
        elem.className = 'processingAlertHide';
    }
   
}

function removeOptions(selectbox) {
    var i;
    for (i = selectbox.options.length - 1; i >= 0; i--) {
        selectbox.remove(i);
    }
}
function removeOptionsExceptFirst(selectbox) {
    var i;
    for (i = selectbox.options.length - 1; i >= 1; i--) {
        selectbox.remove(i);
    }
}

Element.prototype.removeSelectOptions = function() {
    var i;
    for (i = this.options.length - 1; i >= 0; i--) {
        this.remove(i);
    }
};
Element.prototype.removeSelectOptionsExceptFirst = function () {
    var i;
    for (i = this.options.length - 1; i >= 1; i--) {
        this.remove(i);
    }
    this.options[0].defaultSelected = true;
    this.selectedIndex = 0;
    this.style.borderColor = 'black';

};
function S4() {
    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
}
function guid() {
    return (S4() + S4() + "-" + S4() + "-" + S4() + "-" + S4() + "-" + S4() + S4() + S4());
}
function sendSystemMessage(message) {
    $("#systemMessage").text(message);
    $('#systemMessenger').animate({ top: "0%" }, "fast");
    setTimeout(function() {
        $('#systemMessenger').animate({ top: "-30%" }, "fast");
    }, 1500);
    
}

messageContent = function (id) {
    var messages = ["Saved", "Created New Item", "Invalid Permissions", "Welcome!!", "Invalid UserName/Password"];
    return messages[id];
};
selectedObject = function () {
    var items = [],
        setItems = function (itemId, text, objectType, order, active) {
            items = [];
            items.push({
                ItemId: itemId,
                Text: text,
                ObjectType: objectType,
                Order: order,
                Active: active
            });
        },
        getCollection = function () {
            return items;
        },
        setActive = function (active) {
            items[0].Active = active;
        };


    return {
        setItems: setItems,
        getCollection: getCollection,
        setActive: setActive
    };
};
function borderColor(isTrue) {
    var retVal = 'red';
    if (isTrue === 'true') {
        retVal = 'green';
    }
    return retVal;
}

function ajaxRequestFail(data) {
    
    window.location.href = data.Url;
    setTimeout(function() {
        sendSystemMessage(data.Message);
    }, 200);
}
function ajaxMobileRequestFail(data) {
    
    window.location.href = data.Url;
}