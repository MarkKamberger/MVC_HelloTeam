
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