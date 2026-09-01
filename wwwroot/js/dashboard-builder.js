/* Builder 6 edits declarative bindings; mapping/execution lives on the server. */
(function (root) {
  'use strict';
  const escapeHtml = v => String(v ?? '').replace(/[&<>"']/g, c => ({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[c]));
  const safeUrl = v => /^(https?:\/\/|\/(?![\/\\]))/i.test(String(v || '').trim()) ? String(v).trim() : '';
  const targets = {
    donut: [['labels','Nhãn'],['values','Giá trị','number'],['colors','Màu (tùy chọn)']],
    line: [['categories','Trục X'],['values','Giá trị','number'],['group','Nhóm series (tùy chọn)']],
    bar: [['categories','Trục X'],['values','Giá trị','number'],['group','Nhóm series (tùy chọn)']],
    heatmap: [['x','Trục X / tuần'],['y','Trục Y / ngày'],['value','Giá trị ô','number']],
    list: [['name','Tiêu đề'],['description','Mô tả'],['value','Giá trị'],['image','Ảnh'],['pct','Tỷ lệ (%)','number']],
    multiple_mini_card: [['name','Nhãn chỉ tiêu'],['value','Giá trị'],['faclass','Lớp icon'],['color','Màu']],
    emoji_card: [['value','Giá trị'],['unit','Nhãn / đơn vị'],['icon','Icon']],
    large_img_card: [['value1','Giá trị chính'],['value2','Giá trị phụ'],['imageurl','Ảnh']],
    button: [['value','Nội dung'],['url','Đường dẫn']]
  };
  function defaults(kind) {
    let data = {};
    if (kind === 'donut') data = {labels:[],values:[],colors:[]};
    else if (['line','bar'].includes(kind)) data = {categories:[],series:[]};
    else if (kind === 'heatmap') data = {weekLabels:[],days:[],matrix:{}};
    else if (['list','multiple_mini_card','table','report'].includes(kind)) data = {rows:[],columns:[]};
    return {version:1,type:'json',content:JSON.stringify(data,null,2),parameters:[],result:{mode:'direct',path:'$'},mapping:{}};
  }
  function format(value, config) {
    if (value == null) return '';
    const f = config?.numberFormat || {};
    return (typeof value === 'number' ? new Intl.NumberFormat(f.locale || 'vi-VN',{maximumFractionDigits: Math.min(10, Math.max(0,Number(f.decimals ?? 2)))}).format(value) : String(value)) + (f.suffix || '');
  }
  function fields(raw) {
    const found = new Set();
    function visit(value,path,depth) {
      if (depth > 5 || found.size > 200) return;
      if (Array.isArray(value)) { if(value.length) visit(value[0],path ? path+'[0]' : '[0]',depth+1); }
      else if(value && typeof value === 'object') Object.keys(value).forEach(k => { const next=path?path+'.'+k:k; found.add(next); visit(value[k],next,depth+1); });
    }
    visit(raw,'',0);
    if(Array.isArray(raw) && raw[0]) Object.keys(raw[0]).forEach(k=>found.add(k));
    return [...found];
  }
  const rawCache = new Map();
  const drafts = new Map();
  function editor(host, widget, api) {
    const code = widget.id;
    const binding = widget.config.dataBinding ||= defaults(widget.kind);
    binding.result ||= {mode:'direct',path:'$'}; binding.mapping ||= {}; binding.parameters ||= [];
    let tab = widget._bindingTab || 'display';
    const tabs = document.createElement('div'); tabs.className='b6-tabs';
    const display = document.createElement('div'); display.dataset.tab='display';
    // Keep existing visual/chart editors intact in the Display tab.
    while(host.firstChild) display.appendChild(host.firstChild);
    const source=document.createElement('div'), mapping=document.createElement('div');
    host.append(tabs,display,source,mapping);
    const panels={display,source,mapping};
    for(const [key,label] of [['display','Hiển thị'],['source','Dữ liệu'],['mapping','Ánh xạ']]) {
      const b=document.createElement('button');b.type='button';b.textContent=label;
      b.onclick=()=>{tab=key;widget._bindingTab=key;show();}; b.dataset.tab=key; tabs.appendChild(b);
    }
    function show(){Object.entries(panels).forEach(([key,el])=>el.hidden=key!==tab);tabs.querySelectorAll('button').forEach(b=>b.classList.toggle('active',b.dataset.tab===tab));}
    function change(){api.invalidate();api.changed();}
    function field(parent,label,tag='input'){
      const wrap=document.createElement('label');wrap.className='b6-field';const title=document.createElement('span');title.textContent=label;const input=document.createElement(tag);wrap.append(title,input);parent.appendChild(wrap);return input;
    }
    function select(parent,label,values,value,onchange){const input=field(parent,label,'select');values.forEach(([v,t])=>input.add(new Option(t,v)));input.value=value;input.onchange=()=>onchange(input.value);return input;}
    function note(parent,text){const p=document.createElement('p');p.className='b6-note';p.textContent=text;parent.appendChild(p);return p;}
    select(source, 'Loại dữ liệu', [['store', 'store — Procedure'], ['sqlcontent', 'sqlcontent — Truy vấn đọc'], ['json', 'json — Dữ liệu tĩnh']], binding.type, value => {
      binding.type = value;

      if (value === 'json' && binding.result?.mode === 'direct') {
        binding.mapping = {};
      }

      change();
      renderSourceHelp();
    });
    const content=field(source,'Nội dung nguồn','textarea');content.rows=7;content.spellcheck=false;content.value=binding.content || '';
    content.oninput = () => {
      binding.content = content.value;

      syncTableColumnsFromJson();
      renderMapping();
      change();
    };
    const help=note(source,'');
    function renderSourceHelp(){help.textContent=binding.type==='store'?'Nhập schema.tên_store. Tham số cấu hình bên dưới; không nhập CALL. Chỉ gọi store đã được duyệt cho dashboard.':binding.type==='sqlcontent'?'Một SELECT/WITH đọc dữ liệu, không có dấu ; hoặc chú thích. Tham số dùng {{_ten}}. Tối đa 5.000 dòng / 8 giây.':'Nhập JSON thật. Nội dung JSON sẽ được lưu làm nguồn tĩnh.';}
    renderSourceHelp();
    const parameters=document.createElement('div');source.appendChild(parameters);
    function renderParameters(){parameters.replaceChildren();
      binding.parameters.forEach((p,i)=>{
        const row=document.createElement('div');row.className='b6-param';parameters.appendChild(row);
        const name=field(row,'Tham số');name.value=p.name||'';name.placeholder='_userid';name.onchange=()=>{p.name=name.value;change();};
        select(row,'Kiểu',[['integer','integer'],['bigint','bigint'],['text','text'],['numeric','numeric'],['date','date'],['boolean','boolean']],p.type||'text',v=>{p.type=v;change();});
        select(row,'Giá trị từ',[['context.userId','Người dùng đăng nhập'],['constant','Giá trị cố định'],['filter','Bộ lọc']],p.from||'constant',v=>{p.from=v;change();renderParameters();});
        if(p.from!=='context.userId'){const val=field(row,p.from==='filter'?'Tên bộ lọc':'Giá trị');val.value=p.from==='filter'?(p.key||''):(p.value??'');val.onchange=()=>{p[p.from==='filter'?'key':'value']=val.value;change();};}
        const del=document.createElement('button');del.type='button';del.textContent='Xóa tham số';del.onclick=()=>{binding.parameters.splice(i,1);change();renderParameters();};row.appendChild(del);
      });
    }
    renderParameters();const add=document.createElement('button');add.type='button';add.textContent='+ Tham số';add.onclick=()=>{binding.parameters.push({name:'_param',type:'text',from:'constant',value:''});change();renderParameters();};source.appendChild(add);
    const allowed=api.allowedStores || []; if(allowed.length){const details=document.createElement('details');const title=document.createElement('summary');title.textContent='Store được duyệt ('+allowed.length+')';details.appendChild(title);const pre=document.createElement('pre');pre.textContent=allowed.join('\n');details.appendChild(pre);source.appendChild(details);}
    const run=document.createElement('button');run.type='button';run.className='b6-run';run.textContent='Chạy thử nguồn & ánh xạ';source.appendChild(run);
    const run2=run.cloneNode(true);mapping.appendChild(run2);
    const status=note(source,'Chạy thử không lưu cấu hình.');const status2=note(mapping,'');
    const rawView=document.createElement('pre');rawView.className='b6-preview';source.appendChild(rawView);
    const normalized=document.createElement('pre');normalized.className='b6-preview';mapping.appendChild(normalized);
    const controls=document.createElement('div');mapping.insertBefore(controls,run2);
    select(controls,'Cấu trúc đầu vào',[['direct','Đã đúng chuẩn widget'],['rows','Nhiều dòng — chọn cột'],['object','Object — chọn đường dẫn / mảng'],['items','Nhiều chỉ tiêu từ một object']],binding.result.mode,v=>{binding.result.mode=v;change();renderMapping();});
    const path=field(controls,'Đường dẫn gốc');path.value=binding.result.path||'$';path.placeholder='$ hoặc $[0] hoặc $.rows';path.onchange=()=>{binding.result.path=path.value;change();};
    const parse=field(controls,'Cột gốc chứa chuỗi JSON cần parse');parse.type='checkbox';parse.checked=!!binding.result.parseJson;parse.onchange=()=>{binding.result.parseJson=parse.checked;change();};
    const order=field(controls,'Cột sắp xếp dòng (tùy chọn)');order.value=binding.result.orderBy||'';order.onchange=()=>{binding.result.orderBy=order.value;change();};
    const maps = document.createElement('div'); controls.appendChild(maps);
    function syncTableColumnsFromJson() {
      const isJsonTable =
        binding.type === 'json' &&
        ['table', 'report'].includes(widget.kind);

      if (!isJsonTable) return false;

      try {
        const sourceData = JSON.parse(binding.content || '{}');

        if (!Array.isArray(sourceData.columns)) return false;

        const newColumns = sourceData.columns
          .filter(column =>
            column &&
            typeof column.field === 'string' &&
            column.field.trim() !== ''
          )
          .map(column => ({
            field: column.field.trim(),
            label: String(column.label || column.field),
            type: column.type || 'auto'
          }));

        if (newColumns.length === 0) return false;

        binding.columns = newColumns;

        const validFields = new Set(
          newColumns.map(column => column.field)
        );

        // Xóa các mapping cũ không còn thuộc danh sách cột mới.
        Object.keys(binding.mapping || {}).forEach(key => {
          if (!validFields.has(key)) {
            delete binding.mapping[key];
          }
        });

        // direct không sử dụng mapping.
        if (binding.result?.mode === 'direct') {
          binding.mapping = {};
        }

        return true;
      } catch {
        return false;
      }
    }
    function renderMapping() {
      syncTableColumnsFromJson();
      maps.replaceChildren();
      const mode = binding.result.mode;
      if(mode==='direct'){note(maps,'Nguồn phải trả đúng hợp đồng của widget. Thiếu trường/sai kiểu sẽ được báo khi chạy thử.');}
      else if(mode==='items'){
        binding.items ||= [];
        binding.items.forEach((item,i)=>{
          const row=document.createElement('div');row.className='b6-map';maps.appendChild(row);
          const label=field(row,'Nhãn chỉ tiêu '+(i+1));label.value=item.name?.constant||'';
          label.onchange=()=>{item.name={constant:label.value};change();};
          const value=field(row,'Trường giá trị');value.value=item.value?.field||'';
          value.onchange=()=>{item.value={...item.value,field:value.value,type:item.value?.type||'number'};change();};
          const remove=document.createElement('button');remove.type='button';remove.textContent='Xóa chỉ tiêu';remove.onclick=()=>{binding.items.splice(i,1);change();renderMapping();};row.appendChild(remove);
        });
        const addItem=document.createElement('button');addItem.type='button';addItem.textContent='+ Chỉ tiêu';addItem.onclick=()=>{binding.items.push({name:{constant:'Chỉ tiêu'},value:{field:'',type:'number'}});change();renderMapping();};maps.appendChild(addItem);
        note(maps,'Icon, màu, đơn vị và cách chuyển số của từng chỉ tiêu có thể chỉnh trong cấu hình nâng cao.');
      }
      else {
        const available=fields(rawCache.get(code));const list=document.createElement('datalist');list.id='b6-fields-'+code.replace(/[^a-zA-Z0-9]/g,'');available.forEach(s=>list.appendChild(new Option(s,s)));maps.appendChild(list);
        let specs;

        if (
          ['table', 'report'].includes(widget.kind) &&
          Array.isArray(binding.columns) &&
          binding.columns.length > 0
        ) {
          // Bảng lấy danh sách trường đích từ cấu hình cột hiện tại.
          specs = binding.columns
            .filter(column =>
              column &&
              typeof column.field === 'string' &&
              column.field.trim() !== ''
            )
            .map(column => [
              column.field.trim(),
              column.label || column.field,
              column.type || 'auto'
            ]);
        } else {
          // Các widget khác vẫn sử dụng hợp đồng trường cố định.
          specs =
            targets[widget.kind] ||
            Object.keys(binding.mapping).map(key => [key, key, 'auto']);
        }

        if (mode === 'object' && ['line', 'bar'].includes(widget.kind)) {
          specs = [
            ['categories', 'Mảng nhãn trục X'],
            ['series', 'Mảng series / giá trị', 'number']
          ];
        }

        if (mode === 'object' && widget.kind === 'heatmap') {
          specs = [
            ['weekLabels', 'Nhãn trục X'],
            ['days', 'Nhãn trục Y'],
            ['matrix', 'Object ma trận']
          ];
        }
        specs.forEach(([key,label,type])=>{
          const line=document.createElement('div');line.className='b6-map';maps.appendChild(line);
          const input=field(line,label+' → '+key);input.setAttribute('list',list.id);input.placeholder=mode==='rows'?'Chọn / nhập tên cột':'Đường dẫn trường hoặc mảng';input.value=binding.mapping[key]?.field || '';
          const cast=select(line,'Kiểu đầu ra',[['auto','Giữ nguyên'],['string','Chữ'],['number','Số']],binding.mapping[key]?.type || type || 'auto',v=>{binding.mapping[key] ||= {field:input.value};binding.mapping[key].type=v;change();});
          input.onchange=()=>{if(!input.value.trim()) delete binding.mapping[key];else binding.mapping[key]={...binding.mapping[key],field:input.value.trim(),type:cast.value};change();};
          const conversion=document.createElement('details');const title=document.createElement('summary');title.textContent='Chuyển đổi dữ liệu';conversion.appendChild(title);line.appendChild(conversion);
          const delimiter=field(conversion,'Dấu tách chuỗi (để trống nếu đã là mảng)');delimiter.value=binding.mapping[key]?.split||'';
          delimiter.onchange=()=>{binding.mapping[key] ||= {field:input.value,type:cast.value};if(delimiter.value)binding.mapping[key].split=delimiter.value;else delete binding.mapping[key].split;change();};
          select(conversion,'Định dạng chuỗi số',[['invariant','1234.56'],['vi-VN','1.234,56'],['en-US','1,234.56']],binding.mapping[key]?.culture||'invariant',v=>{binding.mapping[key] ||= {field:input.value,type:cast.value};binding.mapping[key].culture=v;change();});
          const scale=field(conversion,'Hệ số nhân');scale.type='number';scale.step='any';scale.value=binding.mapping[key]?.scale??1;scale.onchange=()=>{binding.mapping[key] ||= {field:input.value,type:cast.value};binding.mapping[key].scale=Number(scale.value);change();};
          const strip=field(conversion,'Bỏ hậu tố nguồn (ví dụ: ,k)');strip.value=binding.mapping[key]?.stripSuffix||'';strip.onchange=()=>{binding.mapping[key] ||= {field:input.value,type:cast.value};binding.mapping[key].stripSuffix=strip.value;change();};
        });
        if(['table','report'].includes(widget.kind)) {const btn=document.createElement('button');btn.type='button';btn.textContent='Thêm cột';btn.onclick=()=>{const name=prompt('Mã cột đích (ví dụ: room, amount)');if(name&&/^[a-zA-Z_]\w*$/.test(name)){binding.mapping[name]={field:name,type:'auto'};change();renderMapping();}};maps.appendChild(btn);}
      }
      if(['line','bar','heatmap'].includes(widget.kind)) select(maps,'Khi nhiều dòng trùng một ô',[['','Báo lỗi'],['sum','Cộng'],['avg','Trung bình'],['min','Nhỏ nhất'],['max','Lớn nhất']],binding.aggregate||'',v=>{binding.aggregate=v;change();});
    }
    renderMapping();
    const advanced=document.createElement('details');const summary=document.createElement('summary');summary.textContent='Cấu hình nâng cao (JSON)';advanced.appendChild(summary);mapping.appendChild(advanced);
    note(advanced,'Có thể cấu hình split, culture, stripSuffix, scale, constant/default, items, columns, rowOrder, missingValue. Không chạy JavaScript.');
    const json=field(advanced,'dataBinding','textarea');json.rows=13;json.spellcheck=false;json.value=drafts.get(code) ?? JSON.stringify(binding,null,2);json.oninput=()=>drafts.set(code,json.value);
    advanced.ontoggle=()=>{if(advanced.open&&!drafts.has(code)) json.value=JSON.stringify(binding,null,2);};
    const apply=document.createElement('button');apply.type='button';apply.textContent='Áp dụng cấu hình JSON';apply.onclick=()=>{try{const value=JSON.parse(json.value);if(!value||Array.isArray(value)||typeof value!=='object')throw Error('Cần object JSON');widget.config.dataBinding=value;drafts.delete(code);change();api.rebuild();}catch(e){status2.textContent=e.message;}};advanced.appendChild(apply);
    const formatHost=document.createElement('div');display.prepend(formatHost);widget.config.numberFormat ||= {locale:'vi-VN',decimals:2,suffix:''};
    const decimals=field(formatHost,'Số chữ số thập phân');decimals.type='number';decimals.min=0;decimals.max=10;decimals.value=widget.config.numberFormat.decimals;decimals.onchange=()=>{widget.config.numberFormat.decimals=Math.max(0,Math.min(10,Number(decimals.value)||0));api.render();api.changed();};
    const suffix=field(formatHost,'Hậu tố giá trị (đ, K, %, …)');suffix.value=widget.config.numberFormat.suffix||'';suffix.oninput=()=>{widget.config.numberFormat.suffix=suffix.value;api.render();api.changed();};
    async function preview(){const snapshot=JSON.stringify(widget.config.dataBinding);run.disabled=run2.disabled=true;status.textContent=status2.textContent='Đang lấy dữ liệu…';
      try{const response=await api.preview(widget);if(snapshot!==JSON.stringify(widget.config.dataBinding)){status.textContent=status2.textContent='Cấu hình đã đổi; vui lòng chạy thử lại.';return;}
        rawCache.set(code,response.raw);rawView.textContent=JSON.stringify(response.raw,null,2);normalized.textContent=JSON.stringify(response.dataset?.data,null,2);renderMapping();
        const message=response.errorMessage || response.dataset?.notice || 'Dữ liệu hợp lệ; chưa lưu cấu hình.';status.textContent=status2.textContent=message;
        api.result(response.dataset);
      }catch(e){status.textContent=status2.textContent=e.message;}finally{run.disabled=run2.disabled=false;}
    }
    run.onclick=run2.onclick=preview;
    if(rawCache.has(code))rawView.textContent=JSON.stringify(rawCache.get(code),null,2);
    show();
  }
  root.Dashboard6UI={escapeHtml,safeUrl,defaults,format,editor,targets,hasDrafts:()=>drafts.size>0,clearDraft:id=>{drafts.delete(id);rawCache.delete(id);}};
})(typeof window==='undefined'?globalThis:window);
