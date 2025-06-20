!function(e, t) {
    if ("object" == typeof exports && "object" == typeof module)
        module.exports = t();
    else if ("function" == typeof define && define.amd)
        define([], t);
    else {
        var n = t();
        for (var o in n)
            ("object" == typeof exports ? exports : e)[o] = n[o]
    }
}(self, (function() {
    return function() {
        var e = {
            1009: function(e, t, n) {
                !function e(t, n, o) {
                    function i(a, c) {
                        if (!n[a]) {
                            if (!t[a]) {
                                if (r)
                                    return r(a, !0);
                                var d = new Error("Cannot find module '" + a + "'");
                                throw d.code = "MODULE_NOT_FOUND",
                                d
                            }
                            var s = n[a] = {
                                exports: {}
                            };
                            t[a][0].call(s.exports, (function(e) {
                                return i(t[a][1][e] || e)
                            }
                            ), s, s.exports, e, t, n, o)
                        }
                        return n[a].exports
                    }
                    for (var r = void 0, a = 0; a < o.length; a++)
                        i(o[a]);
                    return i
                }({
                    1: [function(e, t, n) {
                        var o = e("dragula");
                        !function() {
                            this.jKanban = function() {
                                var e = this
                                  , t = {
                                    enabled: !1
                                }
                                  , n = {
                                    enabled: !1
                                };
                                this._disallowedItemProperties = ["id", "title", "click", "drag", "dragend", "drop", "order"],
                                this.element = "",
                                this.container = "",
                                this.boardContainer = [],
                                this.handlers = [],
                                this.dragula = o,
                                this.drake = "",
                                this.drakeBoard = "",
                                this.itemAddOptions = n,
                                this.itemHandleOptions = t;
                                var i = {
                                    element: "",
                                    gutter: "15px",
                                    widthBoard: "250px",
                                    responsive: "700",
                                    responsivePercentage: !1,
                                    boards: [],
                                    dragBoards: !0,
                                    dragItems: !0,
                                    itemAddOptions: n,
                                    itemHandleOptions: t,
                                    dragEl: function(e, t) {},
                                    dragendEl: function(e) {},
                                    dropEl: function(e, t, n, o) {},
                                    dragBoard: function(e, t) {},
                                    dragendBoard: function(e) {},
                                    dropBoard: function(e, t, n, o) {},
                                    click: function(e) {},
                                    buttonClick: function(e, t) {}
                                };
                                function r(t, n) {
                                    t.addEventListener("click", (function(t) {
                                        t.preventDefault(),
                                        e.options.click(this),
                                        "function" == typeof this.clickfn && this.clickfn(this)
                                    }
                                    ))
                                }
                                function a(t, n) {
                                    t.addEventListener("click", (function(t) {
                                        t.preventDefault(),
                                        e.options.buttonClick(this, n)
                                    }
                                    ))
                                }
                                function c(t) {
                                    var n = [];
                                    return e.options.boards.map((function(e) {
                                        if (e.id === t)
                                            return n.push(e)
                                    }
                                    )),
                                    n[0]
                                }
                                function d(t, n) {
                                    for (var o in n)
                                        e._disallowedItemProperties.indexOf(o) > -1 || t.setAttribute("data-" + o, n[o])
                                }
                                function s(t) {
                                    var n = t;
                                    if (e.options.itemHandleOptions.enabled)
                                        if (void 0 === (e.options.itemHandleOptions.customHandler || void 0)) {
                                            var o = e.options.itemHandleOptions.customCssHandler
                                              , i = e.options.itemHandleOptions.customCssIconHandler;
                                            void 0 === (o || void 0) && (o = "drag_handler"),
                                            void 0 === (i || void 0) && (i = o + "_icon"),
                                            n = "<div class='item_handle " + o + "'><i class='item_handle " + i + "'></i></div><div>" + n + "</div>"
                                        } else
                                            n = e.options.itemHandleOptions.customHandler.replace("%s", n);
                                    return n
                                }
                                arguments[0] && "object" == typeof arguments[0] && (this.options = function(e, t) {
                                    var n;
                                    for (n in t)
                                        t.hasOwnProperty(n) && (e[n] = t[n]);
                                    return e
                                }(i, arguments[0])),
                                this.__getCanMove = function(t) {
                                    return e.options.itemHandleOptions.enabled ? e.options.itemHandleOptions.handleClass ? t.classList.contains(e.options.itemHandleOptions.handleClass) : t.classList.contains("item_handle") : !!e.options.dragItems
                                }
                                ,
                                this.init = function() {
                                    !function() {
                                        e.element = document.querySelector(e.options.element);
                                        var t = document.createElement("div");
                                        t.classList.add("kanban-container"),
                                        e.container = t,
                                        document.querySelector(e.options.element).dataset.hasOwnProperty("board") ? (url = document.querySelector(e.options.element).dataset.board,
                                        window.fetch(url, {
                                            method: "GET",
                                            headers: {
                                                "Content-Type": "application/json"
                                            }
                                        }).then((t => {
                                            t.json().then((function(t) {
                                                e.options.boards = t,
                                                e.addBoards(e.options.boards, !0)
                                            }
                                            ))
                                        }
                                        )).catch((e => {}
                                        ))) : e.addBoards(e.options.boards, !0);
                                        e.element.appendChild(e.container)
                                    }(),
                                    window.innerWidth > e.options.responsive && (e.drakeBoard = e.dragula([e.container], {
                                        moves: function(t, n, o, i) {
                                            return !!e.options.dragBoards && (o.classList.contains("kanban-board-header") || o.classList.contains("kanban-title-board"))
                                        },
                                        accepts: function(e, t, n, o) {
                                            return t.classList.contains("kanban-container")
                                        },
                                        revertOnSpill: !0,
                                        direction: "horizontal"
                                    }).on("drag", (function(t, n) {
                                        t.classList.add("is-moving"),
                                        e.options.dragBoard(t, n),
                                        "function" == typeof t.dragfn && t.dragfn(t, n)
                                    }
                                    )).on("dragend", (function(t) {
                                        !function() {
                                            for (var t = 1, n = 0; n < e.container.childNodes.length; n++)
                                                e.container.childNodes[n].dataset.order = t++
                                        }(),
                                        t.classList.remove("is-moving"),
                                        e.options.dragendBoard(t),
                                        "function" == typeof t.dragendfn && t.dragendfn(t)
                                    }
                                    )).on("drop", (function(t, n, o, i) {
                                        t.classList.remove("is-moving"),
                                        e.options.dropBoard(t, n, o, i),
                                        "function" == typeof t.dropfn && t.dropfn(t, n, o, i)
                                    }
                                    )),
                                    e.drake = e.dragula(e.boardContainer, {
                                        moves: function(t, n, o, i) {
                                            return e.__getCanMove(o)
                                        },
                                        revertOnSpill: !0
                                    }).on("cancel", (function(t, n, o) {
                                        e.enableAllBoards()
                                    }
                                    )).on("drag", (function(t, n) {
                                        var o = t.getAttribute("class");
                                        if ("" !== o && o.indexOf("not-draggable") > -1)
                                            e.drake.cancel(!0);
                                        else {
                                            t.classList.add("is-moving"),
                                            e.options.dragEl(t, n);
                                            var i = c(n.parentNode.dataset.id);
                                            void 0 !== i.dragTo && e.options.boards.map((function(t) {
                                                -1 === i.dragTo.indexOf(t.id) && t.id !== n.parentNode.dataset.id && e.findBoard(t.id).classList.add("disabled-board")
                                            }
                                            )),
                                            null !== t && "function" == typeof t.dragfn && t.dragfn(t, n)
                                        }
                                    }
                                    )).on("dragend", (function(t) {
                                        e.options.dragendEl(t),
                                        null !== t && "function" == typeof t.dragendfn && t.dragendfn(t)
                                    }
                                    )).on("drop", (function(t, n, o, i) {
                                        e.enableAllBoards();
                                        var r = c(o.parentNode.dataset.id);
                                        (void 0 !== r.dragTo && -1 === r.dragTo.indexOf(n.parentNode.dataset.id) && n.parentNode.dataset.id !== o.parentNode.dataset.id && e.drake.cancel(!0),
                                        null !== t) && (!1 === e.options.dropEl(t, n, o, i) && e.drake.cancel(!0),
                                        t.classList.remove("is-moving"),
                                        "function" == typeof t.dropfn && t.dropfn(t, n, o, i))
                                    }
                                    )))
                                }
                                ,
                                this.enableAllBoards = function() {
                                    var e = document.querySelectorAll(".kanban-board");
                                    if (e.length > 0 && void 0 !== e)
                                        for (var t = 0; t < e.length; t++)
                                            e[t].classList.remove("disabled-board")
                                }
                                ,
                                this.addElement = function(t, n) {
                                    var o = e.element.querySelector('[data-id="' + t + '"] .kanban-drag')
                                      , i = document.createElement("div");
                                    return i.classList.add("kanban-item"),
                                    void 0 !== n.id && "" !== n.id && i.setAttribute("data-eid", n.id),
                                    n.class && Array.isArray(n.class) && n.class.forEach((function(e) {
                                        i.classList.add(e)
                                    }
                                    )),
                                    i.innerHTML = s(n.title),
                                    i.clickfn = n.click,
                                    i.dragfn = n.drag,
                                    i.dragendfn = n.dragend,
                                    i.dropfn = n.drop,
                                    d(i, n),
                                    r(i),
                                    e.options.itemHandleOptions.enabled && (i.style.cursor = "default"),
                                    o.appendChild(i),
                                    e
                                }
                                ,
                                this.addForm = function(t, n) {
                                    var o = e.element.querySelector('[data-id="' + t + '"] .kanban-drag')
                                      , i = n.getAttribute("class");
                                    return n.setAttribute("class", i + " not-draggable"),
                                    o.appendChild(n),
                                    e
                                }
                                ,
                                this.addBoards = function(t, n) {
                                    if (e.options.responsivePercentage)
                                        if (e.container.style.width = "100%",
                                        e.options.gutter = "1%",
                                        window.innerWidth > e.options.responsive)
                                            var o = (100 - 2 * t.length) / t.length;
                                        else
                                            o = 100 - 2 * t.length;
                                    else
                                        o = e.options.widthBoard;
                                    var i = e.options.itemAddOptions.enabled
                                      , c = e.options.itemAddOptions.content
                                      , l = e.options.itemAddOptions.class
                                      , u = e.options.itemAddOptions.footer;
                                    for (var f in t) {
                                        var p = t[f];
                                        n || e.options.boards.push(p),
                                        e.options.responsivePercentage || ("" === e.container.style.width ? e.container.style.width = parseInt(o) + 2 * parseInt(e.options.gutter) + "px" : e.container.style.width = parseInt(e.container.style.width) + parseInt(o) + 2 * parseInt(e.options.gutter) + "px");
                                        var v = document.createElement("div");
                                        v.dataset.id = p.id,
                                        v.dataset.order = e.container.childNodes.length + 1,
                                        v.classList.add("kanban-board"),
                                        e.options.responsivePercentage ? v.style.width = o + "%" : v.style.width = o,
                                        v.style.marginLeft = e.options.gutter,
                                        v.style.marginRight = e.options.gutter;
                                        var m = document.createElement("header");
                                        if ("" !== p.class && void 0 !== p.class)
                                            var h = p.class.split(",");
                                        else
                                            h = [];
                                        m.classList.add("kanban-board-header"),
                                        h.map((function(e) {
                                            e = e.replace(/^[ ]+/g, ""),
                                            m.classList.add(e)
                                        }
                                        )),
                                        m.innerHTML = '<div class="kanban-title-board">' + p.title + "</div>";
                                        var g = document.createElement("main");
                                        if (g.classList.add("kanban-drag"),
                                        "" !== p.bodyClass && void 0 !== p.bodyClass)
                                            var y = p.bodyClass.split(",");
                                        else
                                            y = [];
                                        for (var b in y.map((function(e) {
                                            g.classList.add(e)
                                        }
                                        )),
                                        e.boardContainer.push(g),
                                        p.item) {
                                            var w = p.item[b]
                                              , T = document.createElement("div");
                                            T.classList.add("kanban-item"),
                                            w.id && (T.dataset.eid = w.id),
                                            w.class && Array.isArray(w.class) && w.class.forEach((function(e) {
                                                T.classList.add(e)
                                            }
                                            )),
                                            T.innerHTML = s(w.title),
                                            T.clickfn = w.click,
                                            T.dragfn = w.drag,
                                            T.dragendfn = w.dragend,
                                            T.dropfn = w.drop,
                                            d(T, w),
                                            r(T),
                                            e.options.itemHandleOptions.enabled && (T.style.cursor = "default"),
                                            g.appendChild(T)
                                        }
                                        var E = document.createElement("footer");
                                        if (i) {
                                            var C = document.createElement("BUTTON")
                                              , O = document.createTextNode(c || "+");
                                            C.setAttribute("class", l || "kanban-title-button btn btn-default btn-xs"),
                                            C.appendChild(O),
                                            u ? E.appendChild(C) : m.appendChild(C),
                                            a(C, p.id)
                                        }
                                        v.appendChild(m),
                                        v.appendChild(g),
                                        v.appendChild(E),
                                        e.container.appendChild(v)
                                    }
                                    return e
                                }
                                ,
                                this.findBoard = function(t) {
                                    return e.element.querySelector('[data-id="' + t + '"]')
                                }
                                ,
                                this.getParentBoardID = function(t) {
                                    return "string" == typeof t && (t = e.element.querySelector('[data-eid="' + t + '"]')),
                                    null === t ? null : t.parentNode.parentNode.dataset.id
                                }
                                ,
                                this.moveElement = function(e, t, n) {
                                    if (e !== this.getParentBoardID(t))
                                        return this.removeElement(t),
                                        this.addElement(e, n)
                                }
                                ,
                                this.replaceElement = function(t, n) {
                                    var o = t;
                                    return "string" == typeof o && (o = e.element.querySelector('[data-eid="' + t + '"]')),
                                    o.innerHTML = n.title,
                                    o.clickfn = n.click,
                                    o.dragfn = n.drag,
                                    o.dragendfn = n.dragend,
                                    o.dropfn = n.drop,
                                    d(o, n),
                                    e
                                }
                                ,
                                this.findElement = function(t) {
                                    return e.element.querySelector('[data-eid="' + t + '"]')
                                }
                                ,
                                this.getBoardElements = function(t) {
                                    return e.element.querySelector('[data-id="' + t + '"] .kanban-drag').childNodes
                                }
                                ,
                                this.removeElement = function(t) {
                                    return "string" == typeof t && (t = e.element.querySelector('[data-eid="' + t + '"]')),
                                    null !== t && ("function" == typeof t.remove ? t.remove() : t.parentNode.removeChild(t)),
                                    e
                                }
                                ,
                                this.removeBoard = function(t) {
                                    var n = null;
                                    "string" == typeof t && (n = e.element.querySelector('[data-id="' + t + '"]')),
                                    null !== n && ("function" == typeof n.remove ? n.remove() : n.parentNode.removeChild(n));
                                    for (var o = 0; o < e.options.boards.length; o++)
                                        if (e.options.boards[o].id === t) {
                                            e.options.boards.splice(o, 1);
                                            break
                                        }
                                    return e
                                }
                                ,
                                this.onButtonClick = function(e) {}
                                ,
                                this.init()
                            }
                        }()
                    }
                    , {
                        dragula: 9
                    }],
                    2: [function(e, t, n) {
                        t.exports = function(e, t) {
                            return Array.prototype.slice.call(e, t)
                        }
                    }
                    , {}],
                    3: [function(e, t, n) {
                        "use strict";
                        var o = e("ticky");
                        t.exports = function(e, t, n) {
                            e && o((function() {
                                e.apply(n || null, t || [])
                            }
                            ))
                        }
                    }
                    , {
                        ticky: 11
                    }],
                    4: [function(e, t, n) {
                        "use strict";
                        var o = e("atoa")
                          , i = e("./debounce");
                        t.exports = function(e, t) {
                            var n = t || {}
                              , r = {};
                            return void 0 === e && (e = {}),
                            e.on = function(t, n) {
                                return r[t] ? r[t].push(n) : r[t] = [n],
                                e
                            }
                            ,
                            e.once = function(t, n) {
                                return n._once = !0,
                                e.on(t, n),
                                e
                            }
                            ,
                            e.off = function(t, n) {
                                var o = arguments.length;
                                if (1 === o)
                                    delete r[t];
                                else if (0 === o)
                                    r = {};
                                else {
                                    var i = r[t];
                                    if (!i)
                                        return e;
                                    i.splice(i.indexOf(n), 1)
                                }
                                return e
                            }
                            ,
                            e.emit = function() {
                                var t = o(arguments);
                                return e.emitterSnapshot(t.shift()).apply(this, t)
                            }
                            ,
                            e.emitterSnapshot = function(t) {
                                var a = (r[t] || []).slice(0);
                                return function() {
                                    var r = o(arguments)
                                      , c = this || e;
                                    if ("error" === t && !1 !== n.throws && !a.length)
                                        throw 1 === r.length ? r[0] : r;
                                    return a.forEach((function(o) {
                                        n.async ? i(o, r, c) : o.apply(c, r),
                                        o._once && e.off(t, o)
                                    }
                                    )),
                                    e
                                }
                            }
                            ,
                            e
                        }
                    }
                    , {
                        "./debounce": 3,
                        atoa: 2
                    }],
                    5: [function(e, t, o) {
                        (function(n) {
                            (function() {
                                "use strict";
                                var o = e("custom-event")
                                  , i = e("./eventmap")
                                  , r = n.document
                                  , a = function(e, t, n, o) {
                                    return e.addEventListener(t, n, o)
                                }
                                  , c = function(e, t, n, o) {
                                    return e.removeEventListener(t, n, o)
                                }
                                  , d = [];
                                function s(e, t, n) {
                                    var o = function(e, t, n) {
                                        var o, i;
                                        for (o = 0; o < d.length; o++)
                                            if ((i = d[o]).element === e && i.type === t && i.fn === n)
                                                return o
                                    }(e, t, n);
                                    if (o) {
                                        var i = d[o].wrapper;
                                        return d.splice(o, 1),
                                        i
                                    }
                                }
                                n.addEventListener || (a = function(e, t, o) {
                                    return e.attachEvent("on" + t, function(e, t, o) {
                                        var i = s(e, t, o) || function(e, t, o) {
                                            return function(t) {
                                                var i = t || n.event;
                                                i.target = i.target || i.srcElement,
                                                i.preventDefault = i.preventDefault || function() {
                                                    i.returnValue = !1
                                                }
                                                ,
                                                i.stopPropagation = i.stopPropagation || function() {
                                                    i.cancelBubble = !0
                                                }
                                                ,
                                                i.which = i.which || i.keyCode,
                                                o.call(e, i)
                                            }
                                        }(e, 0, o);
                                        return d.push({
                                            wrapper: i,
                                            element: e,
                                            type: t,
                                            fn: o
                                        }),
                                        i
                                    }(e, t, o))
                                }
                                ,
                                c = function(e, t, n) {
                                    var o = s(e, t, n);
                                    if (o)
                                        return e.detachEvent("on" + t, o)
                                }
                                ),
                                t.exports = {
                                    add: a,
                                    remove: c,
                                    fabricate: function(e, t, n) {
                                        var a = -1 === i.indexOf(t) ? new o(t,{
                                            detail: n
                                        }) : function() {
                                            var e;
                                            r.createEvent ? (e = r.createEvent("Event")).initEvent(t, !0, !0) : r.createEventObject && (e = r.createEventObject());
                                            return e
                                        }();
                                        e.dispatchEvent ? e.dispatchEvent(a) : e.fireEvent("on" + t, a)
                                    }
                                }
                            }
                            ).call(this)
                        }
                        ).call(this, void 0 !== n.g ? n.g : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {})
                    }
                    , {
                        "./eventmap": 6,
                        "custom-event": 7
                    }],
                    6: [function(e, t, o) {
                        (function(e) {
                            (function() {
                                "use strict";
                                var n = []
                                  , o = ""
                                  , i = /^on/;
                                for (o in e)
                                    i.test(o) && n.push(o.slice(2));
                                t.exports = n
                            }
                            ).call(this)
                        }
                        ).call(this, void 0 !== n.g ? n.g : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {})
                    }
                    , {}],
                    7: [function(e, t, o) {
                        (function(e) {
                            (function() {
                                var n = e.CustomEvent;
                                t.exports = function() {
                                    try {
                                        var e = new n("cat",{
                                            detail: {
                                                foo: "bar"
                                            }
                                        });
                                        return "cat" === e.type && "bar" === e.detail.foo
                                    } catch (e) {}
                                    return !1
                                }() ? n : "undefined" != typeof document && "function" == typeof document.createEvent ? function(e, t) {
                                    var n = document.createEvent("CustomEvent");
                                    return t ? n.initCustomEvent(e, t.bubbles, t.cancelable, t.detail) : n.initCustomEvent(e, !1, !1, void 0),
                                    n
                                }
                                : function(e, t) {
                                    var n = document.createEventObject();
                                    return n.type = e,
                                    t ? (n.bubbles = Boolean(t.bubbles),
                                    n.cancelable = Boolean(t.cancelable),
                                    n.detail = t.detail) : (n.bubbles = !1,
                                    n.cancelable = !1,
                                    n.detail = void 0),
                                    n
                                }
                            }
                            ).call(this)
                        }
                        ).call(this, void 0 !== n.g ? n.g : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {})
                    }
                    , {}],
                    8: [function(e, t, n) {
                        "use strict";
                        var o = {}
                          , i = "(?:^|\\s)"
                          , r = "(?:\\s|$)";
                        function a(e) {
                            var t = o[e];
                            return t ? t.lastIndex = 0 : o[e] = t = new RegExp(i + e + r,"g"),
                            t
                        }
                        t.exports = {
                            add: function(e, t) {
                                var n = e.className;
                                n.length ? a(t).test(n) || (e.className += " " + t) : e.className = t
                            },
                            rm: function(e, t) {
                                e.className = e.className.replace(a(t), " ").trim()
                            }
                        }
                    }
                    , {}],
                    9: [function(e, t, o) {
                        (function(n) {
                            (function() {
                                "use strict";
                                var o = e("contra/emitter")
                                  , i = e("crossvent")
                                  , r = e("./classes")
                                  , a = document
                                  , c = a.documentElement;
                                function d(e, t, o, r) {
                                    n.navigator.pointerEnabled ? i[t](e, {
                                        mouseup: "pointerup",
                                        mousedown: "pointerdown",
                                        mousemove: "pointermove"
                                    }[o], r) : n.navigator.msPointerEnabled ? i[t](e, {
                                        mouseup: "MSPointerUp",
                                        mousedown: "MSPointerDown",
                                        mousemove: "MSPointerMove"
                                    }[o], r) : (i[t](e, {
                                        mouseup: "touchend",
                                        mousedown: "touchstart",
                                        mousemove: "touchmove"
                                    }[o], r),
                                    i[t](e, o, r))
                                }
                                function s(e) {
                                    if (void 0 !== e.touches)
                                        return e.touches.length;
                                    if (void 0 !== e.which && 0 !== e.which)
                                        return e.which;
                                    if (void 0 !== e.buttons)
                                        return e.buttons;
                                    var t = e.button;
                                    return void 0 !== t ? 1 & t ? 1 : 2 & t ? 3 : 4 & t ? 2 : 0 : void 0
                                }
                                function l(e, t) {
                                    return void 0 !== n[t] ? n[t] : c.clientHeight ? c[e] : a.body[e]
                                }
                                function u(e, t, n) {
                                    var o, i = (e = e || {}).className || "";
                                    return e.className += " gu-hide",
                                    o = a.elementFromPoint(t, n),
                                    e.className = i,
                                    o
                                }
                                function f() {
                                    return !1
                                }
                                function p() {
                                    return !0
                                }
                                function v(e) {
                                    return e.width || e.right - e.left
                                }
                                function m(e) {
                                    return e.height || e.bottom - e.top
                                }
                                function h(e) {
                                    return e.parentNode === a ? null : e.parentNode
                                }
                                function g(e) {
                                    return "INPUT" === e.tagName || "TEXTAREA" === e.tagName || "SELECT" === e.tagName || y(e)
                                }
                                function y(e) {
                                    return !!e && ("false" !== e.contentEditable && ("true" === e.contentEditable || y(h(e))))
                                }
                                function b(e) {
                                    return e.nextElementSibling || function() {
                                        var t = e;
                                        do {
                                            t = t.nextSibling
                                        } while (t && 1 !== t.nodeType);
                                        return t
                                    }()
                                }
                                function w(e, t) {
                                    var n = function(e) {
                                        return e.targetTouches && e.targetTouches.length ? e.targetTouches[0] : e.changedTouches && e.changedTouches.length ? e.changedTouches[0] : e
                                    }(t)
                                      , o = {
                                        pageX: "clientX",
                                        pageY: "clientY"
                                    };
                                    return e in o && !(e in n) && o[e]in n && (e = o[e]),
                                    n[e]
                                }
                                t.exports = function(e, t) {
                                    var n, y, T, E, C, O, S, x, k, L, B;
                                    1 === arguments.length && !1 === Array.isArray(e) && (t = e,
                                    e = []);
                                    var N, _ = null, I = t || {};
                                    void 0 === I.moves && (I.moves = p),
                                    void 0 === I.accepts && (I.accepts = p),
                                    void 0 === I.invalid && (I.invalid = function() {
                                        return !1
                                    }
                                    ),
                                    void 0 === I.containers && (I.containers = e || []),
                                    void 0 === I.isContainer && (I.isContainer = f),
                                    void 0 === I.copy && (I.copy = !1),
                                    void 0 === I.copySortSource && (I.copySortSource = !1),
                                    void 0 === I.revertOnSpill && (I.revertOnSpill = !1),
                                    void 0 === I.removeOnSpill && (I.removeOnSpill = !1),
                                    void 0 === I.direction && (I.direction = "vertical"),
                                    void 0 === I.ignoreInputTextSelection && (I.ignoreInputTextSelection = !0),
                                    void 0 === I.mirrorContainer && (I.mirrorContainer = a.body);
                                    var A = o({
                                        containers: I.containers,
                                        start: function(e) {
                                            var t = Y(e);
                                            t && F(t)
                                        },
                                        end: R,
                                        cancel: G,
                                        remove: W,
                                        destroy: function() {
                                            H(!0),
                                            K({})
                                        },
                                        canMove: function(e) {
                                            return !!Y(e)
                                        },
                                        dragging: !1
                                    });
                                    return !0 === I.removeOnSpill && A.on("over", (function(e) {
                                        r.rm(e, "gu-hide")
                                    }
                                    )).on("out", (function(e) {
                                        A.dragging && r.add(e, "gu-hide")
                                    }
                                    )),
                                    H(),
                                    A;
                                    function P(e) {
                                        return -1 !== A.containers.indexOf(e) || I.isContainer(e)
                                    }
                                    function H(e) {
                                        var t = e ? "remove" : "add";
                                        d(c, t, "mousedown", D),
                                        d(c, t, "mouseup", K)
                                    }
                                    function j(e) {
                                        d(c, e ? "remove" : "add", "mousemove", X)
                                    }
                                    function M(e) {
                                        var t = e ? "remove" : "add";
                                        i[t](c, "selectstart", q),
                                        i[t](c, "click", q)
                                    }
                                    function q(e) {
                                        N && e.preventDefault()
                                    }
                                    function D(e) {
                                        if (O = e.clientX,
                                        S = e.clientY,
                                        !(1 !== s(e) || e.metaKey || e.ctrlKey)) {
                                            var t = e.target
                                              , n = Y(t);
                                            n && (N = n,
                                            j(),
                                            "mousedown" === e.type && (g(t) ? t.focus() : e.preventDefault()))
                                        }
                                    }
                                    function X(e) {
                                        if (N)
                                            if (0 !== s(e)) {
                                                if (!(void 0 !== e.clientX && Math.abs(e.clientX - O) <= (I.slideFactorX || 0) && void 0 !== e.clientY && Math.abs(e.clientY - S) <= (I.slideFactorY || 0))) {
                                                    if (I.ignoreInputTextSelection) {
                                                        var t = w("clientX", e) || 0
                                                          , o = w("clientY", e) || 0;
                                                        if (g(a.elementFromPoint(t, o)))
                                                            return
                                                    }
                                                    var i = N;
                                                    j(!0),
                                                    M(),
                                                    R(),
                                                    F(i);
                                                    var u, f = {
                                                        left: (u = T.getBoundingClientRect()).left + l("scrollLeft", "pageXOffset"),
                                                        top: u.top + l("scrollTop", "pageYOffset")
                                                    };
                                                    E = w("pageX", e) - f.left,
                                                    C = w("pageY", e) - f.top,
                                                    r.add(L || T, "gu-transit"),
                                                    function() {
                                                        if (n)
                                                            return;
                                                        var e = T.getBoundingClientRect();
                                                        (n = T.cloneNode(!0)).style.width = v(e) + "px",
                                                        n.style.height = m(e) + "px",
                                                        r.rm(n, "gu-transit"),
                                                        r.add(n, "gu-mirror"),
                                                        I.mirrorContainer.appendChild(n),
                                                        d(c, "add", "mousemove", Q),
                                                        r.add(I.mirrorContainer, "gu-unselectable"),
                                                        A.emit("cloned", n, T, "mirror")
                                                    }(),
                                                    Q(e)
                                                }
                                            } else
                                                K({})
                                    }
                                    function Y(e) {
                                        if (!(A.dragging && n || P(e))) {
                                            for (var t = e; h(e) && !1 === P(h(e)); ) {
                                                if (I.invalid(e, t))
                                                    return;
                                                if (!(e = h(e)))
                                                    return
                                            }
                                            var o = h(e);
                                            if (o)
                                                if (!I.invalid(e, t))
                                                    if (I.moves(e, o, t, b(e)))
                                                        return {
                                                            item: e,
                                                            source: o
                                                        }
                                        }
                                    }
                                    function F(e) {
                                        var t, n;
                                        t = e.item,
                                        n = e.source,
                                        ("boolean" == typeof I.copy ? I.copy : I.copy(t, n)) && (L = e.item.cloneNode(!0),
                                        A.emit("cloned", L, e.item, "copy")),
                                        y = e.source,
                                        T = e.item,
                                        x = k = b(e.item),
                                        A.dragging = !0,
                                        A.emit("drag", T, y)
                                    }
                                    function R() {
                                        if (A.dragging) {
                                            var e = L || T;
                                            z(e, h(e))
                                        }
                                    }
                                    function U() {
                                        N = !1,
                                        j(!0),
                                        M(!0)
                                    }
                                    function K(e) {
                                        if (U(),
                                        A.dragging) {
                                            var t = L || T
                                              , o = w("clientX", e) || 0
                                              , i = w("clientY", e) || 0
                                              , r = J(u(n, o, i), o, i);
                                            r && (L && I.copySortSource || !L || r !== y) ? z(t, r) : I.removeOnSpill ? W() : G()
                                        }
                                    }
                                    function z(e, t) {
                                        var n = h(e);
                                        L && I.copySortSource && t === y && n.removeChild(T),
                                        $(t) ? A.emit("cancel", e, y, y) : A.emit("drop", e, t, y, k),
                                        V()
                                    }
                                    function W() {
                                        if (A.dragging) {
                                            var e = L || T
                                              , t = h(e);
                                            t && t.removeChild(e),
                                            A.emit(L ? "cancel" : "remove", e, t, y),
                                            V()
                                        }
                                    }
                                    function G(e) {
                                        if (A.dragging) {
                                            var t = arguments.length > 0 ? e : I.revertOnSpill
                                              , n = L || T
                                              , o = h(n)
                                              , i = $(o);
                                            !1 === i && t && (L ? o && o.removeChild(L) : y.insertBefore(n, x)),
                                            i || t ? A.emit("cancel", n, y, y) : A.emit("drop", n, o, y, k),
                                            V()
                                        }
                                    }
                                    function V() {
                                        var e = L || T;
                                        U(),
                                        n && (r.rm(I.mirrorContainer, "gu-unselectable"),
                                        d(c, "remove", "mousemove", Q),
                                        h(n).removeChild(n),
                                        n = null),
                                        e && r.rm(e, "gu-transit"),
                                        B && clearTimeout(B),
                                        A.dragging = !1,
                                        _ && A.emit("out", e, _, y),
                                        A.emit("dragend", e),
                                        y = T = L = x = k = B = _ = null
                                    }
                                    function $(e, t) {
                                        var o;
                                        return o = void 0 !== t ? t : n ? k : b(L || T),
                                        e === y && o === x
                                    }
                                    function J(e, t, n) {
                                        for (var o = e; o && !i(); )
                                            o = h(o);
                                        return o;
                                        function i() {
                                            if (!1 === P(o))
                                                return !1;
                                            var i = Z(o, e)
                                              , r = ee(o, i, t, n);
                                            return !!$(o, r) || I.accepts(T, o, y, r)
                                        }
                                    }
                                    function Q(e) {
                                        if (n) {
                                            e.preventDefault();
                                            var t = w("clientX", e) || 0
                                              , o = w("clientY", e) || 0
                                              , i = t - E
                                              , r = o - C;
                                            n.style.left = i + "px",
                                            n.style.top = r + "px";
                                            var a = L || T
                                              , c = u(n, t, o)
                                              , d = J(c, t, o)
                                              , s = null !== d && d !== _;
                                            (s || null === d) && (_ && v("out"),
                                            _ = d,
                                            s && v("over"));
                                            var l = h(a);
                                            if (d !== y || !L || I.copySortSource) {
                                                var f, p = Z(d, c);
                                                if (null !== p)
                                                    f = ee(d, p, t, o);
                                                else {
                                                    if (!0 !== I.revertOnSpill || L)
                                                        return void (L && l && l.removeChild(a));
                                                    f = x,
                                                    d = y
                                                }
                                                (null === f && s || f !== a && f !== b(a)) && (k = f,
                                                d.insertBefore(a, f),
                                                A.emit("shadow", a, d, y))
                                            } else
                                                l && l.removeChild(a)
                                        }
                                        function v(e) {
                                            A.emit(e, a, _, y)
                                        }
                                    }
                                    function Z(e, t) {
                                        for (var n = t; n !== e && h(n) !== e; )
                                            n = h(n);
                                        return n === c ? null : n
                                    }
                                    function ee(e, t, n, o) {
                                        var i = "horizontal" === I.direction
                                          , r = t !== e ? function() {
                                            var e = t.getBoundingClientRect();
                                            if (i)
                                                return a(n > e.left + v(e) / 2);
                                            return a(o > e.top + m(e) / 2)
                                        }() : function() {
                                            var t, r, a, c = e.children.length;
                                            for (t = 0; t < c; t++) {
                                                if (a = (r = e.children[t]).getBoundingClientRect(),
                                                i && a.left + a.width / 2 > n)
                                                    return r;
                                                if (!i && a.top + a.height / 2 > o)
                                                    return r
                                            }
                                            return null
                                        }();
                                        return r;
                                        function a(e) {
                                            return e ? b(t) : t
                                        }
                                    }
                                }
                            }
                            ).call(this)
                        }
                        ).call(this, void 0 !== n.g ? n.g : "undefined" != typeof self ? self : "undefined" != typeof window ? window : {})
                    }
                    , {
                        "./classes": 8,
                        "contra/emitter": 4,
                        crossvent: 5
                    }],
                    10: [function(e, t, n) {
                        var o, i, r = t.exports = {};
                        function a() {
                            throw new Error("setTimeout has not been defined")
                        }
                        function c() {
                            throw new Error("clearTimeout has not been defined")
                        }
                        function d(e) {
                            if (o === setTimeout)
                                return setTimeout(e, 0);
                            if ((o === a || !o) && setTimeout)
                                return o = setTimeout,
                                setTimeout(e, 0);
                            try {
                                return o(e, 0)
                            } catch (t) {
                                try {
                                    return o.call(null, e, 0)
                                } catch (t) {
                                    return o.call(this, e, 0)
                                }
                            }
                        }
                        !function() {
                            try {
                                o = "function" == typeof setTimeout ? setTimeout : a
                            } catch (e) {
                                o = a
                            }
                            try {
                                i = "function" == typeof clearTimeout ? clearTimeout : c
                            } catch (e) {
                                i = c
                            }
                        }();
                        var s, l = [], u = !1, f = -1;
                        function p() {
                            u && s && (u = !1,
                            s.length ? l = s.concat(l) : f = -1,
                            l.length && v())
                        }
                        function v() {
                            if (!u) {
                                var e = d(p);
                                u = !0;
                                for (var t = l.length; t; ) {
                                    for (s = l,
                                    l = []; ++f < t; )
                                        s && s[f].run();
                                    f = -1,
                                    t = l.length
                                }
                                s = null,
                                u = !1,
                                function(e) {
                                    if (i === clearTimeout)
                                        return clearTimeout(e);
                                    if ((i === c || !i) && clearTimeout)
                                        return i = clearTimeout,
                                        clearTimeout(e);
                                    try {
                                        return i(e)
                                    } catch (t) {
                                        try {
                                            return i.call(null, e)
                                        } catch (t) {
                                            return i.call(this, e)
                                        }
                                    }
                                }(e)
                            }
                        }
                        function m(e, t) {
                            this.fun = e,
                            this.array = t
                        }
                        function h() {}
                        r.nextTick = function(e) {
                            var t = new Array(arguments.length - 1);
                            if (arguments.length > 1)
                                for (var n = 1; n < arguments.length; n++)
                                    t[n - 1] = arguments[n];
                            l.push(new m(e,t)),
                            1 !== l.length || u || d(v)
                        }
                        ,
                        m.prototype.run = function() {
                            this.fun.apply(null, this.array)
                        }
                        ,
                        r.title = "browser",
                        r.browser = !0,
                        r.env = {},
                        r.argv = [],
                        r.version = "",
                        r.versions = {},
                        r.on = h,
                        r.addListener = h,
                        r.once = h,
                        r.off = h,
                        r.removeListener = h,
                        r.removeAllListeners = h,
                        r.emit = h,
                        r.prependListener = h,
                        r.prependOnceListener = h,
                        r.listeners = function(e) {
                            return []
                        }
                        ,
                        r.binding = function(e) {
                            throw new Error("process.binding is not supported")
                        }
                        ,
                        r.cwd = function() {
                            return "/"
                        }
                        ,
                        r.chdir = function(e) {
                            throw new Error("process.chdir is not supported")
                        }
                        ,
                        r.umask = function() {
                            return 0
                        }
                    }
                    , {}],
                    11: [function(e, t, n) {
                        (function(e) {
                            (function() {
                                var n;
                                n = "function" == typeof e ? function(t) {
                                    e(t)
                                }
                                : function(e) {
                                    setTimeout(e, 0)
                                }
                                ,
                                t.exports = n
                            }
                            ).call(this)
                        }
                        ).call(this, e("timers").setImmediate)
                    }
                    , {
                        timers: 12
                    }],
                    12: [function(e, t, n) {
                        (function(t, o) {
                            (function() {
                                var i = e("process/browser.js").nextTick
                                  , r = Function.prototype.apply
                                  , a = Array.prototype.slice
                                  , c = {}
                                  , d = 0;
                                function s(e, t) {
                                    this._id = e,
                                    this._clearFn = t
                                }
                                n.setTimeout = function() {
                                    return new s(r.call(setTimeout, window, arguments),clearTimeout)
                                }
                                ,
                                n.setInterval = function() {
                                    return new s(r.call(setInterval, window, arguments),clearInterval)
                                }
                                ,
                                n.clearTimeout = n.clearInterval = function(e) {
                                    e.close()
                                }
                                ,
                                s.prototype.unref = s.prototype.ref = function() {}
                                ,
                                s.prototype.close = function() {
                                    this._clearFn.call(window, this._id)
                                }
                                ,
                                n.enroll = function(e, t) {
                                    clearTimeout(e._idleTimeoutId),
                                    e._idleTimeout = t
                                }
                                ,
                                n.unenroll = function(e) {
                                    clearTimeout(e._idleTimeoutId),
                                    e._idleTimeout = -1
                                }
                                ,
                                n._unrefActive = n.active = function(e) {
                                    clearTimeout(e._idleTimeoutId);
                                    var t = e._idleTimeout;
                                    t >= 0 && (e._idleTimeoutId = setTimeout((function() {
                                        e._onTimeout && e._onTimeout()
                                    }
                                    ), t))
                                }
                                ,
                                n.setImmediate = "function" == typeof t ? t : function(e) {
                                    var t = d++
                                      , o = !(arguments.length < 2) && a.call(arguments, 1);
                                    return c[t] = !0,
                                    i((function() {
                                        c[t] && (o ? e.apply(null, o) : e.call(null),
                                        n.clearImmediate(t))
                                    }
                                    )),
                                    t
                                }
                                ,
                                n.clearImmediate = "function" == typeof o ? o : function(e) {
                                    delete c[e]
                                }
                            }
                            ).call(this)
                        }
                        ).call(this, e("timers").setImmediate, e("timers").clearImmediate)
                    }
                    , {
                        "process/browser.js": 10,
                        timers: 12
                    }]
                }, {}, [1])
            }
        }
          , t = {};
        function n(o) {
            var i = t[o];
            if (void 0 !== i)
                return i.exports;
            var r = t[o] = {
                exports: {}
            };
            return e[o](r, r.exports, n),
            r.exports
        }
        n.n = function(e) {
            var t = e && e.__esModule ? function() {
                return e.default
            }
            : function() {
                return e
            }
            ;
            return n.d(t, {
                a: t
            }),
            t
        }
        ,
        n.d = function(e, t) {
            for (var o in t)
                n.o(t, o) && !n.o(e, o) && Object.defineProperty(e, o, {
                    enumerable: !0,
                    get: t[o]
                })
        }
        ,
        n.g = function() {
            if ("object" == typeof globalThis)
                return globalThis;
            try {
                return this || new Function("return this")()
            } catch (e) {
                if ("object" == typeof window)
                    return window
            }
        }(),
        n.o = function(e, t) {
            return Object.prototype.hasOwnProperty.call(e, t)
        }
        ,
        n.r = function(e) {
            "undefined" != typeof Symbol && Symbol.toStringTag && Object.defineProperty(e, Symbol.toStringTag, {
                value: "Module"
            }),
            Object.defineProperty(e, "__esModule", {
                value: !0
            })
        }
        ;
        var o = {};
        return function() {
            "use strict";
            n.r(o);
            n(1009)
        }(),
        o
    }()
}
));
