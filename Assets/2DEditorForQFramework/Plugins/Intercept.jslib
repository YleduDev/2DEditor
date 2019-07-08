mergeInto(LibraryManager.library, {
    MyURL: function () {
        var url = window.location.host;
        var bufferSize = lengthBytesUTF8(url) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(url, buffer, bufferSize);
        return buffer;
    },
    MyLog: function () {
        var returnStr = "mtyawd";

        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);

        return buffer;

    }
,
    GetText:function () {
        var returnStr =sessionStorage.getItem('text');
        if(returnStr ==null)  return null;
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);

        return buffer;

    }

});


