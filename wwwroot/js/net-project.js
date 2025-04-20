

      // tim kiem trong select box
       $(document).ready(function () {
         function initSelect2() {
             debugger;
             // không áp dụng cho Quiff HTML editor
               $("select") .not(".ql-toolbar select, .ql-container select, .ql-editor select").not(".select2-hidden-accessible").select2({
                   placeholder: "Chọn",
                   allowClear: true,
                   width: "100%"
               });
           }

           // Khởi tạo Select2 cho các select đã có sẵn
           initSelect2();

           // // Áp dụng Select2 cho các select được thêm động
           // $(document).on('focus', 'select', function () {
           //     if (!$(this).hasClass("select2-hidden-accessible")) {
           //         $(this).select2({
           //             placeholder: "Chọn",
           //             allowClear: true,
           //             width: "100%"
           //         });
           //     }
           // });
       });

         // focus vao field dau tien khong hop le
       $("form").on("submit", function (event) {
       var firstInvalid = $(this).find(":invalid").first();
       if (firstInvalid.length) {
       firstInvalid.focus();
       event.preventDefault(); // Ngăn không cho form submit
       }
       });

       // data fill sau khi on change select box
       $(document).ready(function () {
       $("select").change(function () {
       let value = $(this).val();
       let key = $(this).attr("name"); // Lấy name của select
       let datafillstore = $(this).data("datafillstore"); // Lấy giá trị từ data-datafillstore

       if (value && datafillstore && datafillstore != "") {
       $.ajax({
       url: `/HsBookings/GetDataFillSelection`, // Sử dụng serviceStore để xác định API cần gọi
       type: "GET",
       data: { value: value, key: key, datafillstore : datafillstore },
       success: function (data) {
       console.log("Dữ liệu trả về:", data);
       console.log("Loại dữ liệu:", typeof data); // Phải là "object"

       $.each(data, function (key, value) {
       let inputField = $(`[name='${key}'], [id='${key}']`);
       if (inputField.length) {
           if (inputField.is(":radio")) {
               // Tìm radio button có giá trị tương ứng
               let radioToCheck = inputField.filter(`[value='${value}']`);

               if (radioToCheck.length) {
                   // Nếu có radio button phù hợp, chọn nó
                   radioToCheck.prop("checked", true);
               } else {
                   // Nếu không tìm thấy giá trị tương ứng, bỏ chọn tất cả radio của nhóm đó
                   inputField.prop("checked", false);
               }
           } else {
               // Nếu là input bình thường, gán giá trị trực tiếp
               inputField.val(value);
           }
       }
           });
       },
       error: function (xhr, status, error) {
       console.error("Error:", xhr.responseText);
       }
       });
       }
       });
       });

 
       // // xu ly du lieu datetime truyen vao
       // function formatDateTimeLocal(value) {
       //     return value ? moment(value, "M/D/YYYY h:mm:ss A").format("YYYY-MM-DDTHH:mm") : "";
       // }

       // function formatDate(value) {
       //     // Parse the date using moment.js and format it to 'YYYY-MM-DD'
       //     return value ? moment(value, ["M/D/YYYY h:mm:ss A", "YYYY-MM-DDTHH:mm:ss", "YYYY-MM-DD"]).format("YYYY-MM-DD") : "";
       // }

       // function formatTime(value) {
       //     return value ? moment(value, "h:mm:ss A").format("HH:mm") : "";
       // }

       // var formData = @Html.Raw(Json.Serialize(ViewBag.FormData));
       // if (formData != null)
       // {
       //   $(document).ready(function () {
       //       $.each(formData, function (key, value) {
       //           let inputField = $(`[name='${key}'], [id='${key}']`); // Tìm input theo name hoặc id

       //           if (inputField.length) {
       //               let inputType = inputField.attr("type"); // Lấy kiểu input
       //               let isoFormatRegex1 = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}:\d{2}$/; // regex kiem tra dinh dang date YYYY-MM-DDTHH:mm:ss
       //               let isoFormatRegex2 = /^\d{4}-\d{2}-\d{2}$/; // regex kiem tra dinh dang date YYYY-MM-DD
       //               let isoFormatRegex3 = /^\d{2}:\d{2}:\d{2}$/; // regex kiem tra dinh dang date HH:mm

       //               if (inputType === "datetime-local") {
       //                   if (isoFormatRegex1.test(value) == false)
       //                   {
       //                     value = formatDateTimeLocal(value);
       //                     inputField.val(value); // Gán giá trị vào input
       //                   }
       //                   else
       //                   {
       //                     inputField.val(value); // Gán giá trị vào input
       //                     console.log("PaymentDate value:", value)
       //                   }
       //               } 
       //               else if (inputType === "date" && isoFormatRegex2.test(value) == false) {
       //                   value = formatDate(value);
       //                   inputField.val(value); // Gán giá trị vào input
       //               } 
       //               else if (inputType === "time" && isoFormatRegex3.test(value) == false) {
       //                   value = formatTime(value);
       //                   inputField.val(value); // Gán giá trị vào input
       //               }

       //           }
       //       });

       //   });
       // }

       // sau khi load, tu dong cap nhat format date
       $(document).ready(function () {
           // Lấy tất cả các input có type="date" hoặc "datetime-local"
           $('input[type="date"], input[type="datetime-local"]').each(function () {
               let originalValue = $(this).attr('value');

               if (originalValue) {
                   if ($(this).attr("type") === "date") {
                       // Kiểm tra xem đã đúng định dạng YYYY-MM-DD chưa
                       let isoDateRegex = /^\d{4}-\d{2}-\d{2}$/;
                       if (!isoDateRegex.test(originalValue)) {
                           let formattedDate = moment(originalValue, ["M/D/YYYY h:mm:ss A", "DD/MM/YYYY", "YYYY-MM-DD"]).format("YYYY-MM-DD");
                           $(this).val(formattedDate); // Điền trực tiếp vào value
                           $(this).attr("value", formattedDate); // Cập nhật lại giá trị trong HTML
                       }
                   } else if ($(this).attr("type") === "datetime-local") {
                       // Kiểm tra xem đã đúng định dạng YYYY-MM-DDTHH:mm chưa
                       let isoDateTimeRegex = /^\d{4}-\d{2}-\d{2}T\d{2}:\d{2}$/;
                       if (!isoDateTimeRegex.test(originalValue)) {
                           let formattedDateTime = moment(originalValue, ["M/D/YYYY h:mm:ss A", "DD/MM/YYYY HH:mm:ss", "YYYY-MM-DDTHH:mm:ss"]).format("YYYY-MM-DDTHH:mm");
                           $(this).val(formattedDateTime); // Điền trực tiếp vào value
                           $(this).attr("value", formattedDateTime); // Cập nhật lại giá trị trong HTML
                       }
                   }
               }
           });
       });

         // function xoa dòng áp dụng cho report editor
         function toggleDelete(button) {
           var row = $(button).closest("tr");
           var isDeletedInput = row.find(".isDeleted");
           var idInput = row.find("input[name$='Id']").val();

           if (idInput === "" || idInput === null) {
               row.remove(); // Xóa dòng nếu Id rỗng
           } else {
               var isDeleted = isDeletedInput.val() === "true";
               isDeletedInput.val(!isDeleted);

               if (!isDeleted) {
                   $(button).html('<i class="ri-recycle-line ri-24px text-warning"></i>');
                   // ✅ Đánh dấu dòng đã thay đổi (neu co)
                   row.attr("data-changed", "true");
               } else {
                   $(button).html('<i class="ri-delete-bin-line ri-24px text-danger"></i>');
               }
           }
       }

       // kiem tra de xet readonly form
         $(document).ready(function () {
           var isReadOnly = @Json.Serialize(ViewBag.IsReadOnly);
           if (isReadOnly) {
             // Chỉ đặt readonly cho input, textarea
             $("form input, form textarea").prop("readonly", true);

             // Chỉ đặt disable cho select
             $("form select").prop("disabled", true);

             // Chỉ disable nút submit
             $("form button[type='submit']").prop("disabled", true);
           }
       });

       // loai bo disable cho select trong truong hop form submit (ly do khi disabled thi IFormColletion khong lay duoc gia tri)
       $(document).ready(function () {
           $("form").on("submit", function () {
                   $("select:disabled").prop("disabled", false);
               });
       });

       // xu ly spinner khi load
       $(document).ready(function () {
           // Thêm Spinner vào body khi trang tải
           $("body").append(`
           <div id="loading-overlay" style="display: none; position: fixed; top: 0; left: 0; width: 100%; height: 100%; pointer-events: none; background: #96929e;opacity: 0.5;z-index: 2000;display: flex;justify-content: center;align-items: center;pointer-events: none;">
               <div id="spinner" style="background: rgba(0, 0, 0, 0.7);padding: 20px;border-radius: 10px;text-align: center;box-shadow: 0 0 10px rgba(0, 0, 0, 0.2);">
                   <div class="spinner-border text-primary" role="status">
                         <span class="visually-hidden">Loading...</span>
                     </div>
                   <p style="text-align:center; font-weight:bold; color: white;">Đang xử lý...</p>
               </div>
           </div>
           `);

         let loadingTimeout;

         // Chỉ hiển thị spinner sau 2 giây khi trang load
         loadingTimeout = setTimeout(() => {
             $("#loading-overlay").show();
         }, 1000);

         // Khi trang load xong, hủy timeout và ẩn spinner
         $(window).on("load", function () {
             clearTimeout(loadingTimeout);
             $("#loading-overlay").hide();
         });

         // Khi submit form, chỉ hiển thị sau 2 giây
         $(document).on("submit", "form", function () {
             clearTimeout(loadingTimeout);
             loadingTimeout = setTimeout(() => {
                 $("#loading-overlay").show();
             }, 1000);
         });

         // Khi AJAX request gửi đi, chỉ hiển thị sau 2 giây
         $(document).ajaxStart(function () {
             clearTimeout(loadingTimeout);
             loadingTimeout = setTimeout(() => {
                 $("#loading-overlay").show();
             }, 1000);
         });

         // Khi AJAX request hoàn tất, ẩn spinner ngay lập tức
         $(document).ajaxComplete(function () {
             clearTimeout(loadingTimeout);
             $("#loading-overlay").hide();
         });

         // Ẩn spinner nếu có lỗi khi tải trang hoặc request thất bại
         $(window).on("error", function () {
             clearTimeout(loadingTimeout);
             $("#loading-overlay").hide();
         });
       });

       // xu ly confirm action de hien thi popup confirm va xu ly ajax
       $('.confirmAction').on('click', function (e) {
           e.preventDefault(); // Ngăn chặn điều hướng khi click vào link

           var id = $(this).data('id'); // Lấy ID từ thuộc tính data-id
           var sqlstore = $(this).data('sqlstore'); // lay du lieu store de xu ly (bat buoc)
           var confirmtext = $(this).data('confirmtext');

           rplm({
               title: "Xác nhận",
               text: confirmtext || "Hãy giúp tôi xác nhận lại lần nữa nhé",
               type: "warning",
               showCancelButton: true,
               confirmButtonText: "Đồng ý!",
               cancelButtonText: "Hủy!",
               animation: "tada",
               modalNOverlay: "backShadow",
               closeOnConfirm: false,
               closeOnCancel: false,
               showLoaderOnConfirm: true
           },
           function (isConfirm) {
               if (isConfirm) {
                   // dua cac du lieu can truyen vao formdata
                   var formData = new FormData();
                   formData.append("Id", id); // key cua du lieu
                   formData.append("sqlstore", sqlstore); // Lấy sqlstore tu button truyen vao

                   // Gọi AJAX để gửi yêu cầu xóa
                   $.ajax({
                       url: '/Home/Action_Confirmed', // Controller xử lý xóa
                       type: 'POST', // Hoặc 'GET' nếu phù hợp
                       data: formData, // Truyền ID của dòng cần xóa và store xử lý
                       processData: false, // Không chuyển `FormData` thành query string
                       contentType: false, // Để trình duyệt tự động đặt `content-type`
                       success: function (response) {
                           if (response.success) {
                               rplm({
                                 title: "Xử lý thành công!",
                                 text: "Hành động của bạn đã được xử lý.",
                                 type: "success",
                                 timer: 2000,
                                 confirmButtonText: 'Xác nhận',
                               }, function () {
                                     // Sau khi đóng popup, reload lại trang
                                     location.reload();
                                 });
                           } else {
                               rplm({
                                   title: "Lỗi!",
                                   text: response.errorMessage || "Có vấn đề xảy ra.",
                                   type: "error",
                                   timer: 2000,
                                   confirmButtonText: 'Xác nhận',
                               });
                           }
                       },
                       error: function () {
                           rplm({
                               title: "Lỗi!",
                               text: "Lỗi khi kết nối đến máy chủ.",
                               type: "error",
                               timer: 2000,
                               confirmButtonText: 'Done',
                           });
                       }
                   });
               } else {
                   rplm({
                       title: "Đã hủy!",
                       text: "Hành động của bạn đã được hủy.",
                       type: "error",
                       timer: 2000,
                       confirmButtonText: 'Done',
                   });
               }
           });
       });
       //   // xu ly form popup
       //   $(document).ready(function () {
       //     // Khi modal được mở
       //       $(".popup-form-action").click(function () {
       //         var id = $(this).data("id"); // Lấy ID của booking
       //         var sqlStore = $(this).data('sqlstore'); // Lấy SQL Store nếu cần
       //         // neu khong truyen action va controller thi se goi action mac dinh
       //         var controller = $(this).data("controller") || "HsBookings";
       //         var action = $(this).data("action") || "GetDataFillSelection";

       //         // Kiểm tra nếu không có ID thì không làm gì
       //         if (!id) return;

       //         // Gọi AJAX lấy dữ liệu từ action
       //         $.ajax({
       //         url: `/${controller}/${action}`, // Sử dụng serviceStore để xác định API cần gọi
       //         type: "GET",
       //         data: { value: id, key: "Id", datafillstore : sqlStore },
       //         success: function (data) {
       //         console.log("Dữ liệu trả về:", data);
       //         console.log("Loại dữ liệu:", typeof data); // Phải là "object"

       //         $.each(data, function (key, value) {
       //         let inputField = $(`[name='${key}'], [id='${key}']`);
       //         if (inputField.length) {
       //             if (inputField.is(":radio")) {
       //                 // Tìm radio button có giá trị tương ứng
       //                 let radioToCheck = inputField.filter(`[value='${value}']`);

       //                 if (radioToCheck.length) {
       //                     // Nếu có radio button phù hợp, chọn nó
       //                     radioToCheck.prop("checked", true);
       //                 } else {
       //                     // Nếu không tìm thấy giá trị tương ứng, bỏ chọn tất cả radio của nhóm đó
       //                     inputField.prop("checked", false);
       //                 }
       //             }
       //             else if (inputField.is("select"))
       //             {
       //               // gan gia tri selected
       //               let selectedvalue = value
       //               if (inputField.hasClass("select2-hidden-accessible"))
       //               {
       //                inputField.select2("destroy");
       //               }
       //               inputField.empty().append('<option value="">Chọn</option>');  // Xóa các option cũ // Thêm option mặc định

       //               // Thêm các option mới
       //               let selectlistkey = "List"+key
       //               console.log("Dropdown field:", inputField);
       //               console.log("Data list:", data[selectlistkey]);
       //               if (data[selectlistkey] && Array.isArray(data[selectlistkey])) { // Kiểm tra danh sách hợp lệ
       //                   let options = data[selectlistkey].map(item => {
       //                       return new Option(item.name, item.id, false, item.id == selectedvalue);
       //                   });

       //                   inputField.append(options);
       //               } else {
       //                   console.error("Danh sách không hợp lệ hoặc không có dữ liệu.");
       //               }

       //               if (inputField.not(".select2-hidden-accessible"))
       //               {
       //                   $(document).ready(function() {
       //                       inputField.select2({
       //                           placeholder: "Chọn",
       //                           allowClear: true,
       //                           width: "100%",
       //                           dropdownParent: $('#modal-GiamGiaPhatSinh') // Chỉ định modal làm container
       //                       });
       //                   });
       //               }
       //             }
       //             else {
       //                 // Nếu là input bình thường, gán giá trị trực tiếp
       //                 inputField.val(value);
       //             }
       //         }
       //             });
       //         },
       //         error: function (xhr, status, error) {
       //         console.error("Error:", xhr.responseText);
       //         }
       //         });
       //     });

       //     // khong chu dong submit form ma thong qua button de tranh viec form tu xu ly
       //     $(".modal-btn-submit").click(function () {
       //         // $(this).closest("form").submit();
       //         var form = $(this).closest("form").get(0); // Lấy HTMLFormElement

       //         // kiem tra neu co validate thi bao loi va return
       //         if (!form.checkValidity()) {
       //             form.classList.add("was-validated"); // Kích hoạt CSS validation của Bootstrap
       //             return;
       //         }

       //         $(this).closest("form").submit(); // Nếu hợp lệ, submit form
       //     });

       //    // xu ly luu form popup
       //     $(".modal-form").submit(function (e) {
       //         e.preventDefault(); // Ngăn form reload trang

       //         let form = $(this);
       //         let modalId = $(this).data("modal-id");
       //         var formData = new FormData(this); // Tạo FormData từ form

       //         $.ajax({
       //             type: form.attr("method"),
       //             url: form.attr("action"),
       //             data: formData,
       //             processData: false, // Không chuyển `FormData` thành query string
       //             contentType: false, // Để trình duyệt tự động đặt `content-type`
       //             success: function (data) {
       //               if (data.success) {
       //                   rplm({
       //                     title: "Xử lý thành công!",
       //                     text: "Hành động của bạn đã được xử lý.",
       //                     type: "success",
       //                     timer: 2000,
       //                     confirmButtonText: 'Xác nhận',
       //                   }, function (){
       //                         $("#"+modalId+" .modal-btn-close").trigger("click");// Đóng modal
       //                         // Sau khi đóng popup, reload lại trang
       //                         location.reload();
       //                     });
       //               } else {
       //                   rplm({
       //                       title: "Lỗi!",
       //                       text: data.errorMessage || "Có vấn đề xảy ra.",
       //                       type: "error",
       //                       timer: 2000,
       //                       confirmButtonText: 'Xác nhận',
       //                   });
       //               }
       //             },
       //             error: function (xhr, status, error) {
       //                 console.log("Lỗi AJAX:", xhr.responseText); // Log lỗi AJAX
       //                 rplm({
       //                     title: "Lỗi!",
       //                     text: xhr.responseText || "Có vấn đề xảy ra.",
       //                     type: "error",
       //                     timer: 2000,
       //                     confirmButtonText: 'Xác nhận',
       //                 });
       //             }
       //         });
       //     });
       // });

       // kiem tra nếu có nhập liệu trên bất kì dòng nào thì mới enable nút Lưu, và dòng nào nhập liệu thì mới truyền xuống IFormCollection
       document.addEventListener("DOMContentLoaded", function () {
           // Theo dõi tất cả form trên trang
           document.querySelectorAll("form").forEach(form => {
               const table = form.querySelector("table"); // Tìm bảng trong form
               const submitButton = form.querySelector("button[type='submit']");

               if (!table || !submitButton) return; // Bỏ qua nếu không có bảng hoặc nút submit

               function checkChanges() {
                   let hasChanges = false;

                   // neu khong co tr nào co attr data change thi mac dinh hien thi nut Luu
                   if (table.querySelectorAll("tr[data-changed]").length == 0)
                   {
                     submitButton.disabled = false;
                     return;
                   }

                   table.querySelectorAll("tr").forEach(row => {
                       if (row.getAttribute("data-changed") === "true") {
                           hasChanges = true;
                       }
                   });

                   submitButton.disabled = !hasChanges; // Bật/tắt nút submit
               }

               // Theo dõi thay đổi trong bảng (chỉ trong phạm vi form chứa nó)
               table.addEventListener("input", function (event) {
                   let target = event.target;
                   if (target.classList.contains("track-change")) {
                       let row = target.closest("tr");
                       row.setAttribute("data-changed", "true"); // Đánh dấu dòng đã thay đổi
                       checkChanges();
                   }
               });

                 // Nếu người dùng click vào nút trong bảng, đánh dấu dòng là thay đổi
               table.addEventListener("click", function (event) {
                   let target = event.target;

                   // Nếu là button trong bảng
                   if (target.closest("button")) {
                       let row = target.closest("tr");
                       if (row) {
                           row.setAttribute("data-changed", "true"); // Đánh dấu dòng đã thay đổi
                           checkChanges(); // Cập nhật trạng thái nút submit
                       }
                   }
               });

               // Chỉ gử i dữ liệu của bảng đã thay đổi
               form.addEventListener("submit", function (event) {
                   let rows = table.querySelectorAll("tbody tr");

                   rows.forEach(row => {
                     // neu khong khai bao data change thi bo qua, neu co khai bao data change thi kiem tra = false thi xoa 
                       if (row.getAttribute('data-changed') !== undefined && row.getAttribute('data-changed') !== null)
                       {
                         if (row.getAttribute("data-changed") !== "true") {
                           row.remove(); // Xóa các dòng chưa thay đổi khỏi form trước khi submit
                         }
                       }
                   });

                   form.submit();
               });

               checkChanges(); // Kiểm tra trạng thái ban đầu
           });
       });

          // xu ly thong bao
       $(document).ready(function () {
           $.ajax({
               url: `/Notifications/GetNotifications`, // Sử dụng serviceStore để xác định API cần gọi
               type: "GET",
               success: function (data) {
                   let notificationList = $("#notificationList");
                   let badge = $("#notificationBadge");
                   notificationList.empty();
                   let unreadCount = 0;

                   data.forEach(notification => {
                       let item = `
                           <li class="list-group-item list-group-item-action dropdown-notifications-item waves-effect ${notification.IsRead ? 'marked-as-read' : ''}">
                                  <div class="d-flex">
                                      <div class="flex-shrink-0 me-3">
                                          <div class="avatar">
                                              ${notification.Images ? `<img src="${notification.Images}" class="w-px-40 h-auto rounded-circle">` : '<span class="avatar-initial rounded-circle bg-label-danger">N/A</span>'}
                                          </div>
                                      </div>
                                      <div class="flex-grow-1">
                                          <h6 class="small mb-1">${notification.Title}</h6>
                                          <small class="mb-1 d-block text-body">${notification.Content}</small>
                                          <small class="text-muted">${notification.ThoiGian}</small>
                                      </div>
                                   <div class="flex-shrink-0 dropdown-notifications-actions">
                                       <a href="javascript:void(0)" class="dropdown-notifications-read" onclick="markAsRead(${notification.Id})"><span class="badge badge-dot"></span></a>
                                       <a href="javascript:void(0)" class="dropdown-notifications-archive"><span class="ri-close-line"></span></a>
                                   </div>
                                  </div>
                              </li>`;

                       notificationList.append(item);
                       if (!notification.IsRead) unreadCount++;

                       //cap nhat lai event remove notifications
                       document.querySelectorAll(".dropdown-notifications-archive").forEach(t => {
                                 t.addEventListener("click", e => {
                                 t.closest(".dropdown-notifications-item").remove()
                             })
                         })
                   });


                   $('#notificationBadge').html(data.length + " tin nhắn");
                   if (unreadCount > 0) {
                       badge.show();
                   } else {
                       badge.hide();
                   }
               },
               error: function () {
                   console.error("Không thể lấy dữ liệu thông báo.");
               }
           });
       });

       function markAsRead(id) {
           $.ajax({
               url: "/Notifications/MarkAsRead",
               method: "POST",
               data: { id: id },
               success: function (data) {
                   location.reload();
               },
               error: function (response) {
                   console.error("Không thể đánh dấu thông báo đã đọc.");
               }
           });
       }

       // ham xu ly chuyen html thanh text
       function htmlToCleanText(html) {
           const temp = document.createElement("div");
           temp.innerHTML = html;

           function processNode(node) {
               let result = "";

               node.childNodes.forEach((child) => {
                   if (child.nodeType === Node.TEXT_NODE) {
                       result += child.textContent.trim();
                   } else if (child.nodeType === Node.ELEMENT_NODE) {
                       const tag = child.tagName.toLowerCase();

                       if (tag === "br") {
                           result += "\n";
                       } else if (tag === "div") {
                           const inner = processNode(child);
                           if (inner.trim()) result += inner.trim() + "\n"; // Xuống dòng sau mỗi div
                       } else if (tag === "span" || tag === "b" || tag === "i") {
                           result += processNode(child); // Lấy text của các thẻ inline
                       } else {
                           result += processNode(child); // fallback
                       }
                   }
               });

               return result;
           }

           return processNode(temp).trim();
       }

       // ham xu ly xuat excel thong qua exceljs
       function exportTableToExcelJS(selector, filename = "export.xlsx") {
           const table = document.querySelector(selector);
           const workbook = new ExcelJS.Workbook();
           const worksheet = workbook.addWorksheet("Danh sách");

           // kiem tra cot co duoc export khong? (neu isexport = 0 hoac class contain d-none thi khong export)
           const isCellExportable = (el) => {
               const style = window.getComputedStyle(el);
               const isHidden = style.display === "none" || el.classList.contains("d-none");
               const hasAttr = el.hasAttribute("data-isexport");
               const notExport = hasAttr && el.getAttribute("data-isexport") === "False";
               return !isHidden && (!hasAttr || !notExport);
           };

           // tính toán màu để loại bỏ alpha trong argb
           function rgbaToRgbSimulated(r, g, b, a, background = [255, 255, 255]) {
               const alpha = a; // từ 0 đến 1
               const [bgR, bgG, bgB] = background;

               const outR = Math.round(r * alpha + bgR * (1 - alpha));
               const outG = Math.round(g * alpha + bgG * (1 - alpha));
               const outB = Math.round(b * alpha + bgB * (1 - alpha));

               return [outR, outG, outB];
           }
           // to mau header export tuong ung voi mau header cua report
           const rgbToHexARGB = (rgb) => {
             const result = rgb.match(/-?\d+(\.\d+)?/g);;
             if (!result) return "FFFFFFFF"; // fallback trắng

             const [r, g, b, a = "1"] = result.map(Number);
             const [simR, simG, simB] = rgbaToRgbSimulated(r, g, b, a);

             return `FF${simR.toString(16).padStart(2, '0')}${simG.toString(16).padStart(2, '0')}${simB.toString(16).padStart(2, '0')}`.toUpperCase()
         };

           let rowIndex = 1;
           let columnTracker = [];

           // === GHI THEAD ===
           table.querySelectorAll("thead tr").forEach(tr => {
               const row = worksheet.getRow(rowIndex);
               let colIndex = 1;

               tr.querySelectorAll("th").forEach(th => {
                   if (!isCellExportable(th)) return;

                   while (columnTracker[colIndex]) colIndex++;

                   const cell = row.getCell(colIndex);
                   cell.value = th.innerText.trim();
                   cell.font = { bold: true };
                   const style = window.getComputedStyle(th);
                   const bgColor = rgbToHexARGB(style.backgroundColor);

                   cell.fill = {
                       type: 'pattern',
                       pattern: 'solid',
                       fgColor: { argb: bgColor }
                   };
                   cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
                   cell.alignment = { vertical: 'middle', horizontal: 'center', wrapText: true };

                   const colspan = parseInt(th.getAttribute("colspan")) || 1;
                   const rowspan = parseInt(th.getAttribute("rowspan")) || 1;

                   if (colspan > 1 || rowspan > 1) {
                       worksheet.mergeCells(rowIndex, colIndex, rowIndex + rowspan - 1, colIndex + colspan - 1);
                   }

                   if (rowspan > 1) {
                       for (let i = 0; i < colspan; i++) {
                           for (let r = 1; r < rowspan; r++) {
                               columnTracker[colIndex + i] ??= {};
                               columnTracker[colIndex + i][rowIndex + r] = true;
                           }
                       }
                   }

                   colIndex += colspan;
               });

               row.commit();
               rowIndex++;
               columnTracker = columnTracker.map(col => {
                   if (col && col[rowIndex]) {
                       delete col[rowIndex];
                       return col;
                   }
                   return col;
               });
           });

           // === GHI TBODY ===
           const thList = Array.from(table.querySelectorAll("thead th")).filter(isCellExportable);

           table.querySelectorAll("tbody tr").forEach(tr => {
               const row = worksheet.getRow(rowIndex);
               let colIndex = 1;

               Array.from(tr.querySelectorAll("td")).forEach((td, idx) => {
                   const relatedTh = table.querySelector(`thead th:nth-child(${idx + 1})`);
                   if (!isCellExportable(td) || (relatedTh && !isCellExportable(relatedTh))) return;

                   const cell = row.getCell(colIndex++);
                   // kiem tra neu td dang input check box thi get giá trị check được
                   const checkbox = td.querySelector('input[type="checkbox"]');
                   if (checkbox) {
                       cell.value = checkbox.checked ? "✔" : "✘"; // Hoặc "Có" / "Không"
                   } else {
                       const rawText = td.innerText.trim();

                       // Kiểm tra tồn tại attribute
                       const displayType = td.hasAttribute("data-displaytype") ? td.getAttribute("data-displaytype") : null;
                       const displayFormat = td.hasAttribute("data-displayformat") ? td.getAttribute("data-displayformat") : null;

                       // neu kieu date thi format date
                       if (displayType === "date") {
                           cell.value = rawText;
                       // neu la number thi format number
                       } else if (["int", "long", "float"].includes(displayType)) {
                           const numberValue = parseFloat(rawText.replace(/,/g, ""));
                           if (!isNaN(numberValue)) {
                               cell.value = numberValue;
                               cell.numFmt = displayFormat || "#,##0.##";
                           } else {
                               cell.value = rawText;
                           } 
                       } else {
                           // fallback: giữ nguyên như hiện tại
                           cell.value = rawText;
                       }
                   }
                   // lay style tu thuoc tinh tren grid
                   const style = window.getComputedStyle(td);
                   const bgColor = rgbToHexARGB(style.backgroundColor);
                   // Nếu màu nền là đen thì bỏ set màu nền
                   const cellFillColor = (bgColor === "00000000") ? undefined : { type: 'pattern', pattern: 'solid', fgColor: { argb: bgColor } };

                   // text align cho excel
                   const textAlign = style.textAlign;
                   const verticalAlign = style.verticalAlign;

                   cell.fill = cellFillColor;
                   cell.alignment = { vertical: verticalAlign, horizontal: textAlign, wrapText: true };
                   cell.border = { top: { style: 'thin' }, left: { style: 'thin' }, bottom: { style: 'thin' }, right: { style: 'thin' } };
               });

               row.commit();
               rowIndex++;
           });

           // === TỰ CĂN CỘT ===
           worksheet.columns.forEach(column => {
               let maxLength = 10;
               column.eachCell({ includeEmpty: true }, cell => {
                   const text = cell.value?.toString() || "";
                   const firstLine = text.includes("\n") ? text.split("\n")[0] : text;
                   maxLength = Math.max(maxLength, firstLine.length);
               });
               column.width = maxLength + 2;
           });

           // === TẢI FILE ===
           workbook.xlsx.writeBuffer().then(buffer => {
               const blob = new Blob([buffer], { type: "application/octet-stream" });
               saveAs(blob, filename);
           });
       }

       // xu ly nút xuat export, phan trang doi voi table cau hinh class table-with-exportexcel
        function initExcelExportForTable(selector, options = {}) {
           // gia tri mac dinh
            const defaultOptions = {
                header: true,
                footer: true,
                title: "Export",
                filename: "export",
                extension: ".xlsx",
                createEmptyCells: false,
                autoFilter: false,
                sheetName: "Sheet1"
            };

            const config = { ...defaultOptions, ...options };

            $(selector).each(function () {
                $table = $(this);

               // ✅ Quan trọng: Tách template ROW trước khi init DataTable
               let $templateRow = $table.find('tbody tr[data-template="true"]').detach();

                // xu ly data table
                const dt = $table.DataTable({
                    dom: '<"d-flex justify-content-between align-items-center datatable-top-component"<"d-flex gap-2"fl><"d-flex gap-2"B>>irtp',
                    buttons: [
                        {
                            // extend: "excelHtml5",
                            className: "buttons-excel buttons-html5 btn btn-primary",
                            text: '<i class="ri-upload-2-line"></i>',
                             action: function () {
                               dt = $table.DataTable();
                                   dt.page.len(-1).draw(); // -1 = Hiển thị tất cả các dòng

                                   setTimeout(() => {
                                       exportTableToExcelJS('#DataTables_Table_0', config.filename + '.xlsx');
   
                                       // sau khi export xong, trở về số trang cũ
                                       dt.page.len(20).draw(); // hoặc dt.page.len(oldLen).draw();
                                   }, 500);
                             },
                            filename: config.filename,
                            customize: config.customize || null,
                            exportOptions: config.exportOptions || {},
                            header: config.header,
                            footer: config.footer,
                            title: config.title,
                            messageTop: config.messageTop || null,
                            messageBottom: config.messageBottom || null,
                            autoFilter: config.autoFilter,
                            sheetName: config.sheetName,
                            exportOptions: {
                                columns: ':visible',
                                stripHtml: true // Loại bỏ tất cả HTML, chỉ lấy văn bản
                            },
                            customize: function (xlsx) {
                                var sheet = xlsx.xl.worksheets["sheet1.xml"];
                                var rows = $("row", sheet);
                                // Mặc định tô màu header
                                // rows.eq(0).attr("s", "2");
                                // $('row:first c', sheet).attr( 's', '42' );
                                rows.eq(1).find("c").attr("s", "42");
                                
                               // Tô màu cả 2 dòng header nếu có (dòng 0 và dòng 1)
                               rows.eq(0).find("c").attr("s", "42"); // Style 42 là header
                               rows.eq(1).find("c").attr("s", "42");
                            }
                        }
                    ],
                    // phan trang
                    lengthMenu: [10, 20, 50, 100], // Các tùy chọn số lượng bản ghi
                    pageLength: 20, // Mặc định hiển thị 25 bản ghi trên mỗi trang
                    lengthChange: true,
                    // mac dinh sap xep theo controller
                     ordering: true,        // bật tính năng sắp xếp
                     order: [],             // KHÔNG sắp xếp cột nào mặc định
                    // hien thi
                    language: {
                            lengthMenu: "_MENU_",
                            search: "",
                            searchPlaceholder: "Tìm kiếm",
                            info: "Hiển thị dòng <span class='highlight'>_START_</span> đến <span class='highlight'>_END_</span> của <span class='highlight'>_TOTAL_</span> kết quả",
                             paginate: {
                                 previous: "<i class='ri-arrow-left-s-line'></i>",  // hoặc "Trước"
                                 next: "<i class='ri-arrow-right-s-line'></i>"       // hoặc "Tiếp"
                             }
                        },
                   initComplete: function (settings, json) {
                       console.log('Init complete:', settings);
                   },
                   drawCallback: function (settings) {
                       console.log('Draw callback:', settings);
                   },
                   infoCallback: function(settings, start, end, max, total, pre) {
                       // Lấy những hàng được áp dụng bộ lọc từ DataTable (vẫn bao gồm cả hàng có class 'd-none')
                       var nodes = this.api().rows({ filter: 'applied' }).nodes();
                       // Lọc và đếm các hàng không có class 'd-none'
                       var visibleCount = $(nodes).filter(function() {
                           return !$(this).hasClass('d-none');
                       }).length;

                       // tính số dòng không hiển thị
                       var dNoneCount = total - visibleCount

                       // nếu là trang cuối thì end - số dòng không hiển thị
                       if (end == total)
                       {
                         end = visibleCount
                       }
                       // Tùy chỉnh chuỗi hiển thị
                       return "Hiển thị dòng <span class='highlight'>"+start+"</span> đến <span class='highlight'>"+end+"</span> của <span class='highlight'>"+visibleCount+"</span> kết quả";
                   }

                });

              // Di chuyển phần phân trang ra ngoài bảng
              // doi phan trang va cac nut xuat, tìm kiem ra ngoai (yeu cau class table-pagination va table-top-action table-info)
              dt.on('draw', function () {
                  $('.dataTables_paginate').appendTo('.report-pagination');
                  $('.datatable-top-component').appendTo('.report-top-action');
                  $('.dataTables_info').appendTo('.report-info');

                 const $tbody = $table.find('tbody');

                 // Tránh thêm trùng
                 if ($tbody.find('tr[data-template="true"]').length === 0) {
                     $tbody.append($templateRow);
                 }
              });

              // Gọi lần đầu sau khi DataTable load xong
              $('.dataTables_paginate').appendTo('.report-pagination');
              $('.datatable-top-component').appendTo('.report-top-action');
              $('.dataTables_info').appendTo('.report-info');

             const $tbody = $table.find('tbody');

             // Tránh thêm trùng
             if ($tbody.find('tr[data-template="true"]').length === 0) {
                 $tbody.append($templateRow);
             }
            });
        }

        function getFormattedDateTime() {
            var now = new Date();
            var year = now.getFullYear();
            var month = String(now.getMonth() + 1).padStart(2, '0');
            var day = String(now.getDate()).padStart(2, '0');
            var hours = String(now.getHours()).padStart(2, '0');
            var minutes = String(now.getMinutes()).padStart(2, '0');
            var seconds = String(now.getSeconds()).padStart(2, '0');

            return `${year}${month}${day}_${hours}${minutes}${seconds}`;
        }

        // xu ly nút xuat export, phan trang doi voi table cau hinh class table-with-exportexcel
        $(document).ready(function () {
            // Lấy nội dung title từ thẻ H1
            var pageTitle = $("#header-title").text().trim();

            // Xoá các ký tự đặc biệt không hợp lệ trong tên file (ví dụ: !, 🎉, ...)
            var sanitizedFilename = pageTitle.replace(/[^a-zA-Z0-9\u00C0-\u1EF9\s]/g, '').replace(/\s+/g, '_');

            // tim table co class table-with-exportexcel va xu ly
            initExcelExportForTable('.table-with-exportexcel', {
                // Thêm timestamp vào filename
                filename: `${sanitizedFilename}_${getFormattedDateTime()}` ?? "Danh sách",
                title: pageTitle ?? "Danh sách",
                autoFilter: false,
                sheetName: "Danh sách"
            });

        });

       // xu ly import file excel de cap nhat va them moi du lieu
        $(document).ready(function () {
          // Khi bấm nút → kích hoạt chọn file
          document.getElementById("importFileBtn").addEventListener("click", function () {
              document.getElementById("importFileInput").click();
          });

          // Khi người dùng chọn file → gửi file lên API
          document.getElementById("importFileInput").addEventListener("change", async function () {
              const file = this.files[0];
              if (!file) return;

              var sqlstore = $(this).data('sqlstore'); // lay du lieu store tu input import de xu ly (bat buoc)

              const formData = new FormData();
              formData.append("file", file);
              formData.append("sqlstore", sqlstore);

              // const resultDiv = document.getElementById("result");
              // resultDiv.innerHTML = "Đang xử lý...";
              rplm({
                  title: "Xác nhận",
                  text: "Nhập file sẽ mất một chút thời gian nhé",
                  type: "warning",
                  showCancelButton: true,
                  confirmButtonText: "Đồng ý!",
                  cancelButtonText: "Hủy!",
                  animation: "tada",
                  modalNOverlay: "backShadow",
                  closeOnConfirm: false,
                  closeOnCancel: false,
                  showLoaderOnConfirm: true
              },
              function (isConfirm) {
                  if (isConfirm) {
                      // Gọi AJAX để gửi yêu cầu xóa
                      $.ajax({
                          url: '/Home/ImportExcel', // Controller xử lý xóa
                          type: 'POST', // Hoặc 'GET' nếu phù hợp
                          data: formData, // Truyền ID của dòng cần xóa và store xử lý
                          processData: false, // Không chuyển `FormData` thành query string
                          contentType: false, // Để trình duyệt tự động đặt `content-type`
                          success: function (response) {
                              if (response.success) {
                                  rplm({
                                    title: "Xử lý thành công!",
                                    text: "Hành động của bạn đã được xử lý.",
                                    type: "success",
                                    timer: 2000,
                                    confirmButtonText: 'Xác nhận',
                                  }, function () {
                                        // Sau khi đóng popup, reload lại trang
                                        location.reload();
                                    });
                              } else {
                                  rplm({
                                      title: "Lỗi!",
                                      text: response.errorMessage || "Có vấn đề xảy ra.",
                                      type: "error",
                                      timer: 2000,
                                      confirmButtonText: 'Xác nhận',
                                  });
                              }
                          },
                          error: function () {
                              rplm({
                                  title: "Lỗi!",
                                  text: "Lỗi khi kết nối đến máy chủ.",
                                  type: "error",
                                  timer: 2000,
                                  confirmButtonText: 'Done',
                              });
                          }
                      });
                  } else {
                      rplm({
                          title: "Đã hủy!",
                          text: "Hành động của bạn đã được hủy.",
                          type: "error",
                          timer: 2000,
                          confirmButtonText: 'Done',
                      });
                  }
              });
          });
        });

       // xử lý Quill HTML editor cho field form
       document.addEventListener("DOMContentLoaded", function () {
       var quillEditors = {}; // Lưu danh sách editor
           document.querySelectorAll(".quill-editor").forEach(function (el) {
               var id = el.id.replace("editor-container-", ""); // lấy fieldName
               var hiddenInput = document.getElementById("editor-content-" + id);

               // Khởi tạo Quill
               var quill = new Quill("#" + el.id, {
                   theme: "snow",
                   placeholder: "Nhập nội dung tại đây...",
                   modules: {
                       toolbar: [
                           ['bold', 'italic', 'underline', 'strike'],        // toggled buttons
                           ['blockquote', 'code-block'],
                           ['link', 'image', 'video', 'formula'],

                           [{ 'header': 1 }, { 'header': 2 }],               // custom button values
                           [{ 'list': 'ordered'}, { 'list': 'bullet' }, { 'list': 'check' }],
                           [{ 'script': 'sub'}, { 'script': 'super' }],      // superscript/subscript
                           [{ 'indent': '-1'}, { 'indent': '+1' }],          // outdent/indent
                           [{ 'direction': 'rtl' }],                         // text direction

                           [{ 'size': ['small', false, 'large', 'huge'] }],  // custom dropdown
                           [{ 'header': [1, 2, 3, 4, 5, 6, false] }],

                           [{ 'color': [] }, { 'background': [] }],          // dropdown with defaults from theme
                           [{ 'font': [] }],
                           [{ 'align': [] }],

                           ['clean']                                         // remove formatting button
                         ]
                   }
               });

               // Gán giá trị từ input nếu có (khi edit)
               if (hiddenInput.value) {
                   quill.root.innerHTML = hiddenInput.value;
               }

               // Lưu vào danh sách
               quillEditors[id] = quill;
           });

           // Khi submit: đồng bộ nội dung
           document.querySelector("form").addEventListener("submit", function () {
               for (var id in quillEditors) {
                   var editor = quillEditors[id];
                   var html = editor.root.innerHTML;
                   document.getElementById("editor-content-" + id).value = html;
               }
           });
       });

       // Hàm xử lý thay đổi cho input file
        $(document).ready(function () {
           const fileMap = @Html.Raw(Json.Serialize(ViewBag.FileUrls)); // { "KOAAttachment": ["/path/file1.png", ...] }
           function createFileListItem(name, url = null, file = null, input = null) {
               const listItem = $('<li class="mb-1 d-flex align-items-center justify-content-between"></li>');
               const fileName = $('<span class="me-2 text-truncate" style="max-width: 200px;"></span>').text(name);
               const btnGroup = $('<div class="btn-group btn-group-sm" role="group"></div>');

               const downloadBtn = $('<button type="button" class="btn btn-outline-primary">Tải</button>');
               const deleteBtn = $('<button type="button" class="btn btn-outline-danger">Xóa</button>');

               downloadBtn.on('click', function () {
                   if (url) {
                       window.open(url, '_blank');
                   } else if (file) {
                       const blobUrl = URL.createObjectURL(file);
                       const a = $('<a></a>').attr('href', blobUrl).attr('download', file.name).css('display', 'none');
                       $('body').append(a);
                       a[0].click();
                       a.remove();
                       URL.revokeObjectURL(blobUrl);
                   }
               });

               deleteBtn.on('click', function () {
                   listItem.remove();
                   if (input) {
                       input.value = "";
                   }
               });

               btnGroup.append(downloadBtn, deleteBtn);
               listItem.append(fileName, btnGroup);
               return listItem;
           }

           // function cập nhật slide mỗi khi chọn lại file
           function updateCarouselFromFiles(carouselSelector, files) {
               const $carousel = $(carouselSelector);
               const $inner = $carousel.find('.carousel-inner');
               const $indicators = $carousel.find('.carousel-indicators');

               $inner.empty();
               $indicators.empty();

               let slideIndex = 0;
               for (let i = 0; i < files.length; i++) {
                   const file = files[i];
                   const fileName = file.name;
                   const fileExt = fileName.toLowerCase().split('.').pop();

                   if (['jpg', 'jpeg', 'png', 'gif'].includes(fileExt)) {
                       const imgURL = URL.createObjectURL(file);

                       const itemClass = slideIndex === 0 ? 'carousel-item active' : 'carousel-item';

                       const $item = $(`
                           <div class="${itemClass}">
                               <img class="d-block w-100 carousel-blur-img" src="${imgURL}" alt="${fileName}" />
                               <div class="carousel-caption d-none d-md-block text-shadow">
                                   <h3><a class="text-white" href="${imgURL}" target="_blank">${fileName}</a></h3>
                               </div>
                           </div>
                       `);

                       const $indicator = $(`<button type="button"
                                                  data-bs-target="${carouselSelector}"
                                                  data-bs-slide-to="${slideIndex}"
                                                  class="${slideIndex === 0 ? "active" : ""}"
                                                  aria-current="${slideIndex === 0 ? "true" : "false"}"
                                                  aria-label="Slide ${slideIndex + 1}"></button>`);

                       $inner.append($item);
                       $indicators.append($indicator);
                       slideIndex++;
                   }
               }

               // Reset lại slide về slide đầu tiên
               const carouselInstance = bootstrap.Carousel.getOrCreateInstance($carousel[0]);
               carouselInstance.to(0);
           }

           function handleFileInputChange(input) {
               let targetSelector = $(input).data("target");
               const carouselSelector = $(input).data("carousel");
               if (!targetSelector) return;

               let fileList = $(targetSelector);
               fileList.empty(); // Xoá toàn bộ

               const validImageFiles = [];

               $.each(input.files, function (index, file) {
                   const listItem = createFileListItem(file.name, null, file, input);
                   fileList.append(listItem);

                   // Nếu là ảnh thì thêm vào mảng để update slide
                   const ext = file.name.toLowerCase().split('.').pop();
                   if (['jpg', 'jpeg', 'png', 'gif'].includes(ext)) {
                       validImageFiles.push(file);
                   }
               });
               // Nếu có carousel liên quan thì cập nhật lại slide
               if (carouselSelector && validImageFiles.length > 0) {
                   updateCarouselFromFiles(carouselSelector, validImageFiles);
               }
           }

           // Load từ server
           if (typeof fileMap !== "undefined") {
               for (const inputName in fileMap) {
                   const filePaths = fileMap[inputName];
                   const fileListId = "#fileList-" + inputName;

                   if (Array.isArray(filePaths)) {
                       const container = $(fileListId);
                       $.each(filePaths, function (_, filePath) {
                           const fileName = filePath.split('/').pop();
                           const listItem = createFileListItem(fileName, filePath);
                           container.append(listItem);
                       });
                   }
               }
           }

           // Apply change handler cho tất cả input file có data-target
           $('input[type="file"][data-target]').on('change', function () {
               handleFileInputChange(this);
           });
       });

       // xử lý control List selection của form
       document.addEventListener("DOMContentLoaded", function () {
           document.querySelectorAll(".selection-list").forEach(list => {
               list.addEventListener("click", function (e) {
                   const item = e.target.closest(".selection-item");
                   if (!item) return;

                   item.classList.toggle("selected");

                   const selectedValues = Array.from(list.querySelectorAll(".selection-item.selected"))
                       .map(el => el.dataset.value);

                   const hiddenInputId = list.dataset.target;
                   document.getElementById(hiddenInputId).value = selectedValues.join(",");
               });
           });
       });


       // xử lý custom tagbox
       $(function() {
           // --- Khởi tạo Tagify cho tất cả input có class 'tag-input' ---
           $(".tag-input").each(function () {
                   const raw = $(this).attr("data-jsonwhitelist") || "";
               // const whitelist = raw.split(",").map(x => x.trim()); // Chuyển chuỗi thành mảng
               let whitelist = [];

               try {
                   whitelist = JSON.parse(raw);
                   console.log("WHITELIST PARSED:", whitelist);
               } catch (e) {
                   console.warn("Invalid whitelist JSON", e);
               }

               new Tagify(this, {
                   placeholder: "Có thể nhập hoặc chọn danh mục có sẵn",
                   whitelist: whitelist,
                   tagTextProp: "label", // <-- Cái này rất quan trọng!
                   originalInputValueFormat: valuesArr => valuesArr.map(tag => tag.value).join(','), // chỉ lưu value
                   dropdown: {
                       enabled: 0,
                       closeOnSelect: true,
                       maxItems: 10,
                       position: "text", // hoặc 'manual' nếu muốn kiểm soát chặt chẽ
                       highlightFirst: true,
                       mapValueTo: "label",    // ← dùng 'label' để hiển thị trong dropdown
                   }
               });
           });

       });

     // thuc hien chuc nang scroll to top (lên đầu trang)
     document.getElementById("scrollToTopBtn").addEventListener("click", function (e) {
     e.preventDefault(); // ngăn chuyển trang nếu href="#"
     window.scrollTo({
       top: 0,
       behavior: "smooth"
     });
   });


   // xu ly du lieu ngay trong report editor
   $(document).ready(function () {
     var gridData = @Html.Raw(Json.Serialize(ViewBag.reportResultList));
     if (gridData != null)
     {
       $.each(gridData, function (index, rowData) {
           // Tìm tất cả input/select trong dòng có name động
           $(`#ReportEditor tbody tr:eq(${index})`).find("input, select, textarea").each(function () {
               let fieldName = $(this).attr("name"); // Lấy name của input
               if (!fieldName) return; // Nếu không có name, bỏ qua

               // Regex để lấy tên key từ `grid[index].FieldName`
               let match = fieldName.match(/\[(\d+)\]\.(\w+)/);
               if (!match) return; // Nếu không khớp format, bỏ qua

               let fieldKey = match[2]; // Lấy phần FieldName (ví dụ: "ServiceID", "Quantity",...)

               if (rowData.hasOwnProperty(fieldKey)) {
                   let value = rowData[fieldKey];

                   // Kiểm tra kiểu input và format lại nếu cần
                   if ($(this).attr("type") === "datetime-local") {
                       value = formatDateTimeLocal(value);
                       // Gán giá trị vào input
                       $(this).val(value);
                   } else if ($(this).attr("type") === "date") {
                       value = formatDate(value);
                       // Gán giá trị vào input
                       $(this).val(value);
                   } else if ($(this).attr("type") === "time") {
                       value = formatTime(value);
                       // Gán giá trị vào input
                       $(this).val(value);
                   }
                   // Nếu là select2, cần trigger để cập nhật giao diện
                   if ($(this).is("select")) {
                       $(this).trigger("change");
                   }
               }
           });
         });
     }
   });


// tự động add param từ link vào hidden input
 window.addEventListener("DOMContentLoaded", () => {
   const urlParams = new URLSearchParams(window.location.search);
   const hiddenDiv = document.getElementById("hiddenInputs");

   urlParams.forEach((value, key) => {
     const input = document.createElement("input");
     input.type = "hidden";
     input.name = "q_"+key;
     input.value = value;
     hiddenDiv.appendChild(input);
   });
 });
