
openBlankWindow = (uri) => {
    window.open(uri, '_blank');
}

showToastrNotification = (status, orderId) => {
    toastr.success(`Updated to status: ${status}`, `Order Id: ${orderId}`);
}
