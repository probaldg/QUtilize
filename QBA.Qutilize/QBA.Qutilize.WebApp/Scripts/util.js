function ShowMessage(message,typeOf) {
    if (message !="") {
        $.notify({
            message: message
        }, {
            z_index: 3000,
            type: typeOf
        });

        return false;
    }
}