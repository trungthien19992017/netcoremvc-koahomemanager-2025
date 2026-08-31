# DashboardBuilder6

Bản riêng của DashboardBuilder5, dùng ba bảng đang có:

- `dbo.net_dashboard`: bố cục, datasource, chủ sở hữu và danh sách store được duyệt.
- `dbo.net_widget`: cấu hình từng widget trong `configjson.dataBinding`.
- `dbo.net_widgettemplate`: tên, loại, shape và giới hạn kích thước widget.

Không sửa schema ba bảng, không thay các store/màn hình cũ. Dashboard thử là `KoaDashboard6`, sao chép từ `KoaDashboard1`. Quyền bản đầu: chỉ chủ sở hữu dashboard được mở, chạy thử và lưu; không tự mở quyền cho tất cả tài khoản đã đăng nhập.

## Chạy

Build/chạy lại ứng dụng rồi mở `/Dashboards/DashboardBuilder6?dashboardCode=KoaDashboard6`.

Chọn widget → **Dữ liệu** → chọn `store`, `sqlcontent` hoặc `json` → nhập nội dung → **Chạy thử nguồn & ánh xạ**. Có thể xem dữ liệu thô ngay cả khi ánh xạ chưa đúng. Sang **Ánh xạ**, chọn cấu trúc đầu vào và các trường đích, chạy thử lại, rồi **Lưu cấu hình**.

- `store`: nhập `dbo.ten_procedure`, không nhập CALL; thêm tham số tên `_...`, kiểu và nguồn giá trị.
- `sqlcontent`: một SELECT/WITH đọc dữ liệu. Ví dụ `select roomname as label, roomid as value from dbo.hs_room order by roomid`. Không nhập dấu chấm phẩy/chú thích; một số hàm báo cáo được cho phép, hàm tùy ý và truy cập catalog hệ thống bị chặn. SQL chạy trong READ ONLY với role không đăng nhập `koa_dashboard6_reader`, tối đa 8 giây/5.000 dòng. Không dùng SQL này cho thao tác cập nhật dữ liệu.
- `json`: JSON tĩnh được lưu nguyên trong cấu hình. Nội dung ví dụ cho Donut: `{"labels":["A","B"],"values":[12,8],"colors":["#16C47F","#FFD65A"]}`.

Tham số SQL dùng `{{_userid}}`, không dùng phép nối chuỗi. Khai báo tham số `_userid` kiểu integer, từ người dùng đăng nhập. `_userid` luôn do server xác định, kể cả khi request cố gửi giá trị khác. Tham số `constant` lấy từ cấu hình; `filter` lấy từ bộ lọc truyền vào store. Bản giao diện này chưa thêm thanh lọc ngày/homestay toàn dashboard; có thể dùng giá trị cố định trong bảng tham số.

## Ánh xạ

| Chế độ | Kết quả nguồn | Cấu hình |
|---|---|---|
| `direct` | Object đúng hợp đồng widget | Chọn đường dẫn gốc, không cần mapping |
| `rows` | Mảng object | `mapping` ánh xạ cột mỗi dòng |
| `object` | Một object, có thể chứa mảng hoặc chuỗi CSV | `mapping` ánh xạ từng trường/đường dẫn |
| `items` | Một object chứa nhiều chỉ tiêu | `items` khai báo từng chỉ tiêu |

Store/SQL trả mảng dòng: `$` là toàn bộ mảng; `$[0]` là dòng đầu. Nếu một cột chứa chuỗi JSON, chọn `$[0].ten_cot` và bật parse JSON. Đường dẫn chỉ hỗ trợ tên trường, dấu chấm và chỉ số mảng, không hỗ trợ biểu thức/chạy JavaScript.

Quy tắc ví dụ:

```json
{
  "version": 1,
  "type": "store",
  "content": "dbo.ten_store_da_duoc_duyet",
  "parameters": [{"name":"_userid","type":"integer","from":"context.userId"}],
  "result": {"mode":"rows","path":"$"},
  "mapping": {
    "labels": {"field":"roomname","type":"string"},
    "values": {"field":"totalamount","type":"number"},
    "colors": {"field":"color","type":"string"}
  }
}
```

Các tùy chọn chuyển đổi đều do cấu hình quyết định:

- `split`: dấu tách chuỗi; không tự tách mọi dấu phẩy.
- `culture`: `invariant`, `vi-VN`, `en-US`; chỉ áp dụng cho chuỗi, số JSON giữ nguyên.
- `stripSuffix`, `scale`: bỏ hậu tố nguồn và nhân hệ số.
- `constant`, `default`: giá trị cố định hoặc khi thiếu/null. Không tự thay lỗi thành 0.
- `display`: định dạng đầu ra thành chuỗi cho thẻ/list, ví dụ `{"decimals":0,"suffix":" đ"}`; không dùng cho series cần số.
- `indexPrefix`: tạo nhãn thứ tự khi chủ động cấu hình trên chuỗi đã tách. Hai store tháng cũ chưa có nhãn tháng, nên bản sao dùng `Kỳ 1...` được khai báo rõ trong mapping; không suy đoán tháng lịch. Khi store có nhãn, thay mapping `categories`.
- `aggregate`: `sum`, `avg`, `min`, `max` cho ô trùng trong Line/Bar/Heatmap; không khai báo thì báo lỗi khi trùng.
- `missingValue`: giá trị ô thiếu; mặc định null.
- `rowOrder`: thứ tự trục Y Heatmap; không cố định theo các thứ trong tuần.
- `result.orderBy`: cột sắp xếp các dòng. Nên trả khóa thứ tự đầy đủ (ngày gồm năm), không chỉ số tuần khi qua năm.
- `columns`: danh sách `{field,label}` cho table/report.

Hợp đồng hiển thị:

| Widget | Dữ liệu đích |
|---|---|
| Donut | `labels[]`, `values[]`, `colors[]` tùy chọn |
| Line/Bar | `categories[]`, `series:[{name,data:[]}]`; chế độ rows ánh xạ `categories`, `values`, `group` tùy chọn |
| Heatmap | `weekLabels[]`, `days[]`, `matrix:{day:[]}`; chế độ rows ánh xạ `x`,`y`,`value` |
| Table/Report | `columns:[{field,label}]`, `rows:[{...}]` |
| List | `rows:[{name,description,value,image,pct}]` |
| Thẻ nhiều chỉ tiêu | `rows:[{name,value,faclass,color}]` |
| Thẻ ảnh | `value1`, `value2`, `imageurl` tùy chọn |
| Thẻ số | `value`, `unit`, `icon` tùy chọn |
| Button | `value`, `url`; hoặc cấu hình tĩnh |

## Store được duyệt

`net_dashboard.options.allowedStores` là danh sách tên procedure đã được quản trị viên duyệt. Bản sao ban đầu có 10 store đang dùng. Đây là cấu hình database, không phải danh sách tên cột/store viết cứng trong bộ ánh xạ. Khi thêm store mới, quản trị viên cần kiểm tra nó chỉ phục vụ báo cáo và thêm tên đủ schema vào danh sách này. SaveLayout6 không cho trình duyệt tự sửa danh sách duyệt.

Store con chạy trong transaction trên cùng kết nối dblink, đọc `tmp_result` rồi ROLLBACK để không giữ lại thay đổi từ việc xem báo cáo. Store cần chỉ tạo/đọc kết quả và không tự COMMIT; rollback không thể hoàn tác tác động bên ngoài database của một procedure, vì vậy vẫn cần duyệt store.

Không lưu dữ liệu chạy thử vào `chartconfig`; ngoại lệ: nội dung nguồn JSON tĩnh phải được lưu trong dataBinding. Khi source/mapping thay đổi, dữ liệu cũ trên canvas được bỏ và yêu cầu chạy thử lại. Chạy thử và tải trang đều qua `Dashboard6Mapper` phía server.

## Triển khai nơi khác

1. Xem lại datasource, quyền và cấu hình ánh xạ ban đầu.
2. `01-reader-role.sql`: chạy trong database nghiệp vụ của datasource. Tài khoản datasource phải là thành viên role đọc; script cấp membership cho tài khoản thực hiện.
3. `02-procedures.sql`: chạy trong `TTT_Config`, tạo hai procedure mới, không sửa hai procedure cũ. Chỉ tài khoản ứng dụng nội bộ được EXECUTE; PUBLIC bị thu hồi quyền.
4. `03-clone-dashboard.sql`: chạy trong `TTT_Config` một lần. Nếu đã có `KoaDashboard6` thì script dừng, không ghi đè.

`initial-bindings.json` chỉ là cấu hình ban đầu để chuyển dashboard hiện có. Bộ ánh xạ và renderer không dùng file này lúc chạy. Thay store/tên cột chỉ cần chỉnh trong giao diện/configjson.

## Kiểm tra

```text
dotnet run --project tests/dashboard6/Dashboard6.Tests.csproj
node --test tests/dashboard6-view.test.cjs
```

`tests/dashboard6/database-roundtrip.sql` kiểm tra cả ba nguồn, lưu/mở lại, tham số, quyền đọc, từ chối SQL nguy hiểm và bảo vệ dashboard gốc; toàn bộ thay đổi dữ liệu thử nằm trong transaction ROLLBACK.

Kiểm tra JavaScript dùng DOM/Apex stub, không thay thế kiểm tra trực quan trên trình duyệt. Trong phiên triển khai này, trình duyệt tích hợp bị chặn khi mở localhost.
