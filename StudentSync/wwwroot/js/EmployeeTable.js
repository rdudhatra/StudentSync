    //$(document).ready(function () {
    //    $('#EmployeeTable').DataTable({
    //        "processing": true,
    //        "serverSide": true,
    //        "ajax": {
    //            "url": "/Employee/GetAll",
    //            "type": "POST"
    //        },
    //        "columns": [
    //            { "data": "id" },
    //            { "data": "firstName" },
    //            { "data": "lastName" },
    //            { "data": "gender" },
    //            { "data": "dob" },
    //            { "data": "qualification" },
    //            { "data": "designation" },
    //            {
    //                "data": null,
    //                "render": function (data, type, row) {
    //                    return '<a href="/Employee/Edit/' + row.id + '">Edit</a> | <a href="/Employee/Details/' + row.id + '">Details</a> | <a href="#" onclick="deleteEmployee(' + row.id + ');">Delete</a>';
    //                }
    //            }
    //        ]
    //    });
    //});

    //function deleteEmployee(id) {
    //    if (confirm("Are you sure you want to delete this employee?")) {
    //    $.ajax({
    //        url: '/Employee/Delete/' + id,
    //        type: 'POST',
    //        success: function (result) {
    //            // Reload DataTable after deletion
    //            $('#EmployeeTable').DataTable().ajax.reload();
    //        },
    //        error: function (xhr, status, error) {
    //            console.error(xhr.responseText);
    //            alert('Error deleting employee');
    //        }
    //    });
    //    }
    //}
