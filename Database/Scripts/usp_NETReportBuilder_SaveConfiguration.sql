/* PostgreSQL - TTT_Config. Backend CALLs this procedure then reads tmp_result. */
CREATE OR REPLACE PROCEDURE dbo.usp_netreportbuilder_saveconfiguration
(
    IN _reportcode text,
    IN _configjson text,
    IN _userid bigint DEFAULT NULL
)
LANGUAGE plpgsql
AS $procedure$
DECLARE
    v_json jsonb;
    v_reportid integer;
    v_actionlistid integer;
    v_now timestamp without time zone := clock_timestamp();
BEGIN
    DROP TABLE IF EXISTS pg_temp.tmp_result;
    CREATE TEMP TABLE tmp_result
    (
        success boolean, errorcode text, errormessage text,
        reportid integer, reportcode text, savedversion timestamp,
        displaysaved integer, filtersaved integer, actionsaved integer
    ) ON COMMIT PRESERVE ROWS;

    BEGIN
        IF NULLIF(btrim(_reportcode), '') IS NULL THEN
            RAISE EXCEPTION 'Mã báo cáo không được để trống.' USING ERRCODE = '22023';
        END IF;
        v_json := _configjson::jsonb;

        SELECT id INTO v_reportid
        FROM dbo.net_report
        WHERE code = _reportcode AND NOT COALESCE(isdeleted, false)
        LIMIT 1;

        IF v_reportid IS NULL THEN
            INSERT INTO dbo.net_report
            (code, name, sqltype, sqlcontent, datasourceid, sqleditcontent,
             sqldefaultcontent, isexportexcel, selectiontype, issearchbar,
             showtoolbar, pagination, creationtime, creatoruserid, isdeleted)
            VALUES
            (_reportcode, v_json #>> '{table,title}', false,
             v_json #>> '{table,sqlContent}', NULLIF(v_json #>> '{table,datasourceId}', '')::bigint,
             v_json #>> '{table,sqlEditContent}', v_json #>> '{table,sqlDefaultContent}',
             COALESCE((v_json #>> '{table,exportExcel}')::boolean, true),
             CASE WHEN COALESCE((v_json #>> '{table,rowSelection}')::boolean, false) THEN 'multiple' ELSE 'none' END,
             COALESCE((v_json #>> '{table,showSearchBar}')::boolean, true),
             COALESCE((v_json #>> '{table,showToolbar}')::boolean, true),
             COALESCE((v_json #>> '{table,pagination}')::boolean, true),
             v_now, _userid, false)
            RETURNING id INTO v_reportid;
        ELSE
            IF COALESCE((v_json #>> '{table,createNew}')::boolean, false) THEN
                RAISE EXCEPTION 'Mã báo cáo đã tồn tại.' USING ERRCODE = '23505';
            END IF;
            UPDATE dbo.net_report SET
                name = v_json #>> '{table,title}', sqlcontent = v_json #>> '{table,sqlContent}',
                datasourceid = NULLIF(v_json #>> '{table,datasourceId}', '')::bigint,
                sqleditcontent = v_json #>> '{table,sqlEditContent}',
                sqldefaultcontent = v_json #>> '{table,sqlDefaultContent}',
                isexportexcel = COALESCE((v_json #>> '{table,exportExcel}')::boolean, true),
                selectiontype = CASE WHEN COALESCE((v_json #>> '{table,rowSelection}')::boolean, false) THEN 'multiple' ELSE 'none' END,
                issearchbar = COALESCE((v_json #>> '{table,showSearchBar}')::boolean, true),
                showtoolbar = COALESCE((v_json #>> '{table,showToolbar}')::boolean, true),
                pagination = COALESCE((v_json #>> '{table,pagination}')::boolean, true),
                lastmodificationtime = v_now, lastmodifieruserid = _userid
            WHERE id = v_reportid;
        END IF;

        CREATE TEMP TABLE display_input ON COMMIT DROP AS
        SELECT x."databaseId" AS databaseid, x.key, x.title,
               x."sourceType" AS sourcetype, x.renderer, x.format, x.width,
               x.colnum, x.visible, x.align, x.aggregate, x."fixed",
               x."groupId" AS groupid, x.isreadonly, x.isexport,
               x."serviceId" AS serviceid, x.cssheader, x.sortable,
               false AS isparent
        FROM jsonb_to_recordset(COALESCE(v_json->'columns','[]')) AS x
        ("databaseId" integer, key text, title text, "sourceType" text, renderer text,
         format text, width integer, colnum integer, visible boolean, align text,
         aggregate text, "fixed" text, "groupId" text, isreadonly boolean,
         isexport boolean, "serviceId" integer, cssheader text, sortable boolean);
        INSERT INTO display_input
        SELECT g."databaseId",g.id,g.title,'string','string',NULL,NULL,NULL,true,'center',NULL,NULL,NULL,true,false,NULL,g.cssheader,false,true
        FROM jsonb_to_recordset(COALESCE(v_json->'groups','[]')) AS g("databaseId" integer,id text,title text,cssheader text);

        UPDATE dbo.net_display d SET name=i.title, format=i.format,
            type=COALESCE(NULLIF(i.sourcetype,''),i.renderer), width=i.width::text,
            colnum=i.colnum, isdisplay=COALESCE(i.visible,true), textalign=i.align,
            issum=(i.aggregate='sum'), isfreepane=(i."fixed"='left'), isparent=i.isparent,
            parentcode=i.groupid, isreadonly=COALESCE(i.isreadonly,false),
            isexport=COALESCE(i.isexport,true), serviceid=i.serviceid, cssheader=i.cssheader,
            issort=COALESCE(i.sortable,false), isdeleted=false, deletiontime=NULL,
            deleteruserid=NULL, lastmodificationtime=v_now,lastmodifieruserid=_userid
        FROM display_input i WHERE d.id=i.databaseid AND d.reportid=v_reportid;
        UPDATE dbo.net_display d SET isdeleted=true,deletiontime=v_now,deleteruserid=_userid
        WHERE d.reportid=v_reportid AND NOT COALESCE(d.isdeleted,false)
          AND NOT EXISTS(SELECT 1 FROM display_input i WHERE i.databaseid=d.id);
        INSERT INTO dbo.net_display
        (reportid,reportcode,code,name,format,type,width,colnum,isdisplay,textalign,
         issum,isfreepane,isparent,parentcode,isreadonly,isexport,serviceid,cssheader,
         issort,creationtime,creatoruserid,isdeleted)
        SELECT v_reportid,_reportcode,key,title,format,COALESCE(NULLIF(sourcetype,''),renderer),
               width::text,colnum,COALESCE(visible,true),align,(aggregate='sum'),("fixed"='left'),
               isparent,groupid,COALESCE(isreadonly,false),COALESCE(isexport,true),serviceid,
               cssheader,COALESCE(sortable,false),v_now,_userid,false
        FROM display_input WHERE databaseid IS NULL;

        CREATE TEMP TABLE filter_input ON COMMIT DROP AS
        SELECT f."databaseId" AS databaseid, f.field, f.label,
               f."dynamicFieldId" AS dynamicfieldid,
               f."serviceId" AS serviceid, f.required, f.enabled,
               f."orderId" AS orderid, f."colSpan" AS colspan
        FROM jsonb_to_recordset(COALESCE(v_json->'filters','[]')) AS f
        ("databaseId" integer,field text,label text,"dynamicFieldId" bigint,"serviceId" bigint,
         required boolean,enabled boolean,"orderId" integer,"colSpan" integer);
        UPDATE dbo.net_filter f SET code=i.field,name=i.label,dynamicfieldid=i.dynamicfieldid,
            seviceid=i.serviceid,required=COALESCE(i.required,false),isactive=COALESCE(i.enabled,true),
            orderid=i.orderid,colspan=i.colspan,isdeleted=false,deletiontime=NULL,deleteruserid=NULL,
            lastmodificationtime=v_now,lastmodifieruserid=_userid
        FROM filter_input i
        WHERE f.reportid=v_reportid
          AND (f.id=i.databaseid OR (i.databaseid IS NULL AND f.code=i.field));
        UPDATE dbo.net_filter f SET isdeleted=true,deletiontime=v_now,deleteruserid=_userid
        WHERE f.reportid=v_reportid AND NOT COALESCE(f.isdeleted,false)
          AND NOT EXISTS
          (
              SELECT 1 FROM filter_input i
              WHERE i.databaseid=f.id OR (i.databaseid IS NULL AND i.field=f.code)
          );
        INSERT INTO dbo.net_filter
        (reportid,reportcode,dynamicfieldid,seviceid,code,name,combolevel,datatype,
         disable,required,version,orderid,isactive,colspan,colcount,isgrouped,
         isloadmultipleway,creationtime,creatoruserid,isdeleted)
        SELECT v_reportid,_reportcode,dynamicfieldid,serviceid,field,label,0,
               false,false,COALESCE(required,false),0,orderid,COALESCE(enabled,true),
               COALESCE(colspan,4),0,false,false,v_now,_userid,false
        FROM filter_input i
        WHERE i.databaseid IS NULL
          AND NOT EXISTS
          (
              SELECT 1 FROM dbo.net_filter f
              WHERE f.reportid=v_reportid AND f.code=i.field
          );

        SELECT id INTO v_actionlistid FROM dbo.net_actionlist
        WHERE objectcode=_reportcode AND actionlisttypecode='REPORT' AND NOT COALESCE(isdeleted,false) LIMIT 1;
        IF v_actionlistid IS NULL
           AND jsonb_array_length(COALESCE(v_json->'actions', '[]'::jsonb)) > 0 THEN
            INSERT INTO dbo.net_actionlist
            (objectid,objectcode,actionlisttypeid,actionlisttypecode,code,name,
             isactive,isdeleted,creationtime,creatoruserid)
            VALUES
            (v_reportid,_reportcode,2,'REPORT',_reportcode,v_json#>>'{table,title}',
             true,false,v_now,_userid::integer)
            RETURNING id INTO v_actionlistid;
        END IF;
        CREATE TEMP TABLE action_input ON COMMIT DROP AS
        SELECT a."detailId" AS detailid, a."actionId" AS actionid,
               a.code,a.name,a.type,a.value,a.icon,a.scope,
               a."requiresSelection" AS requiresselection,a.confirm,a.enabled,
               a."orderId" AS orderid,a."dataSourceId" AS datasourceid
        FROM jsonb_to_recordset(COALESCE(v_json->'actions','[]')) AS a
        ("detailId" integer,"actionId" integer,code text,name text,type text,value text,icon text,
         scope text,"requiresSelection" boolean,confirm boolean,enabled boolean,"orderId" integer,"dataSourceId" integer);
        INSERT INTO dbo.net_action(code,name,descriptions,isactive,isdelete,icon)
        SELECT DISTINCT i.code,i.name,'Action được tạo từ NET Report Builder',COALESCE(i.enabled,true),false,i.icon
        FROM action_input i WHERE i.actionid IS NULL AND NULLIF(btrim(i.code),'') IS NOT NULL
          AND NOT EXISTS(SELECT 1 FROM dbo.net_action a WHERE a.code=i.code AND NOT COALESCE(a.isdelete,false));
        UPDATE action_input i SET actionid=a.id FROM dbo.net_action a
        WHERE i.actionid IS NULL AND a.code=i.code AND NOT COALESCE(a.isdelete,false);
        UPDATE dbo.net_actionlistdetail d SET actionid=i.actionid,displayname=i.name,value=i.value,type=i.type,
            icon=i.icon,istop=(i.scope='top'),ischoosedata=COALESCE(i.requiresselection,false),
            ispopupconfirm=COALESCE(i.confirm,false),isactive=COALESCE(i.enabled,true),orderid=i.orderid,
            datasourceid=i.datasourceid,isdeleted=false,deletiontime=NULL,deleteuserid=NULL,
            lastmodificationtime=v_now,lastmodifieruserid=_userid::integer
        FROM action_input i WHERE d.id=i.detailid AND d.actionlistid=v_actionlistid;
        UPDATE dbo.net_actionlistdetail d SET isdeleted=true,deletiontime=v_now,deleteuserid=_userid::integer
        WHERE d.actionlistid=v_actionlistid AND NOT COALESCE(d.isdeleted,false)
          AND NOT EXISTS(SELECT 1 FROM action_input i WHERE i.detailid=d.id);
        INSERT INTO dbo.net_actionlistdetail
        (actionlistid,actionlistcode,actionid,displayname,value,type,icon,istop,ischoosedata,
         ispopupconfirm,isactive,isdeleted,orderid,datasourceid,creationtime,creatoruserid)
        SELECT v_actionlistid,_reportcode,actionid,name,value,type,icon,(scope='top'),
               COALESCE(requiresselection,false),COALESCE(confirm,false),COALESCE(enabled,true),false,
               orderid,datasourceid,v_now,_userid::integer FROM action_input WHERE detailid IS NULL;

        INSERT INTO tmp_result SELECT true,NULL,NULL,v_reportid,_reportcode,v_now,
            (SELECT COUNT(*)::integer FROM display_input),(SELECT COUNT(*)::integer FROM filter_input),
            (SELECT COUNT(*)::integer FROM action_input);
    EXCEPTION WHEN OTHERS THEN
        INSERT INTO tmp_result VALUES(false,SQLSTATE,SQLERRM,NULL,_reportcode,NULL,0,0,0);
    END;
END;
$procedure$;
