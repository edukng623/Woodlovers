function postToS3() {
    var image = $("#preview-image").attr("src");
    // Split the base64 string in data and contentType
    const block = image.split(";");
    // Get the content type of the image
    const contentType = block[0].split(":")[1];// In this case "image/gif"
    // get the real base64 content of the file
    const realData = block[1].split(",")[1];// In this case "R0lGODlhPQBEAPeoAJosM...."

    // Convert it to a blob to upload
    const blob = b64toBlob(realData, contentType);

    const formDataToUpload = new FormData();
    formDataToUpload.append("image", blob);
    const name = $("#file-name").val();
    formDataToUpload.append("name", name);
    
    $.ajax({
        type: "POST",
        url: '/api/Halftoner/PostToS3',
        contentType: false,
        processData: false,
        data: formDataToUpload,
        success: function (result) {
            console.log(result);
        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3 + " " + p4;
            if (xhr.responseText && xhr.responseText[0] === "{")
                err = JSON.parse(xhr.responseText).Message;
            console.log(err);
        }
    });
}
function onFileSelected(event) {
    

    const loading = $("#loading-div");
    const selectedFile = event.target.files[0];
    const imageContainer = $("#image-container");
    preview(selectedFile);
    imageContainer.hide();
    loading.show();

    const reader = new FileReader();
    
    reader.onload = function (event) {
        const imgtag = document.getElementById("preview-image");
        imgtag.src = event.target.result;
        imageContainer.show();
        loading.hide();
    };

    reader.readAsDataURL(selectedFile);
}
function preview(file) {
    //const loading = $("#loading-div");
    //const imageContainer = $("#image-container");
    //const imgtag = document.getElementById("preview-image");
    //imageContainer.hide();

    
    //// Split the base64 string in data and contentType
    //const block = image.split(";");
    //// Get the content type of the image
    //const contentType = block[0].split(":")[1];// In this case "image/gif"
    //// get the real base64 content of the file
    //const realData = block[1].split(",")[1];// In this case "R0lGODlhPQBEAPeoAJosM...."

    //// Convert it to a blob to upload
    //const blob = b64toBlob(realData, contentType);

    // Create a FormData and append the file with "image" as parameter name
    const loading = $("#loading-div-halftoner");
    const imageContainer = $("#image-container-halftoner");
    
    loading.show();

    const formDataToUpload = new FormData();
    formDataToUpload.append("image", file);
    formDataToUpload.append("settings-file", "Halftoner_60_90.cfg");
    $.ajax({
        type: "POST",
        url: '/api/Halftoner/Edit',
        contentType: false,
        processData: false,
        data: formDataToUpload,
        success: function (result) {
            imageContainer.show();
            $("#preview-image-haltoner").attr("src", 'data:image/png;base64,' + result);
            loading.hide();
        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3 + " " + p4;
            if (xhr.responseText && xhr.responseText[0] === "{")
                err = JSON.parse(xhr.responseText).Message;
            console.log(err);
        }
    });
    //$.ajax({
    //    url: "/api/Halftoner",
    //    data: JSON.stringify({ image: realData }),// Add as Data the Previously create formData
    //    type: "POST",
    //    contentType: "multipart/form-data",
    //    error: function (err) {
    //        console.error(err);
    //    },
    //    success: function (data) {
    //        console.log(data);
    //        imgtag.src = image;
    //        loading.hide();
    //        imageContainer.show();

    //    },
    //    complete: function () {
    //        console.log("Request finished.");
    //    }
    //});
}

function b64toBlob(b64Data, contentType, sliceSize) {
    contentType = contentType || '';
    sliceSize = sliceSize || 512;

    var byteCharacters = atob(b64Data);
    var byteArrays = [];

    for (var offset = 0; offset < byteCharacters.length; offset += sliceSize) {
        var slice = byteCharacters.slice(offset, offset + sliceSize);

        var byteNumbers = new Array(slice.length);
        for (var i = 0; i < slice.length; i++) {
            byteNumbers[i] = slice.charCodeAt(i);
        }

        var byteArray = new Uint8Array(byteNumbers);

        byteArrays.push(byteArray);
    }

    var blob = new Blob(byteArrays, { type: contentType });
    return blob;
}


function postOrder() {
    const order = $("#order-number").val();
    const files = $("#files").val();

    const formDataToUpload = new FormData();
    formDataToUpload.append("order", order);

    formDataToUpload.append("files", files);
    $.ajax({
        type: "POST",
        url: '/api/Halftoner/PostOrder',
        contentType: false,
        processData: false,
        data: formDataToUpload,
        success: function (result) {
            console.log(result);
        },
        error: function (xhr, status, p3, p4) {
            var err = "Error " + " " + status + " " + p3 + " " + p4;
            if (xhr.responseText && xhr.responseText[0] == "{")
                err = JSON.parse(xhr.responseText).Message;
            console.log(err);
            console.log(xhr.responseText);
            console.log(err);
        }
    });
}