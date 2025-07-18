document.addEventListener("DOMContentLoaded", function() {
    {
        var r = (e, t={
            wheelPropagation: !1,
            suppressScrollX: !0
        }) => {
            e && new PerfectScrollbar(e,t)
        }
        ;
        let a = {
            emailList: document.querySelector(".email-list"),
            emailListItems: Array.from(document.querySelectorAll(".email-list-item")),
            emailListItemInputs: Array.from(document.querySelectorAll(".email-list-item-input")),
            emailView: document.querySelector(".app-email-view-content"),
            emailFilters: document.querySelector(".email-filters"),
            emailFilterByFolders: Array.from(document.querySelectorAll(".email-filter-folders li")),
            emailEditor: document.querySelector(".email-editor"),
            appEmailSidebar: document.querySelector(".app-email-sidebar"),
            appOverlay: document.querySelector(".app-overlay"),
            emailReplyEditor: document.querySelector(".email-reply-editor"),
            bookmarkEmail: Array.from(document.querySelectorAll(".email-list-item-bookmark")),
            selectAllEmails: document.getElementById("email-select-all"),
            emailSearch: document.querySelector(".email-search-input"),
            toggleCC: document.querySelector(".email-compose-toggle-cc"),
            toggleBCC: document.querySelector(".email-compose-toggle-bcc"),
            emailCompose: document.querySelector(".app-email-compose"),
            emailListDelete: document.querySelector(".email-list-delete"),
            emailListRead: document.querySelector(".email-list-read"),
            emailListEmpty: document.querySelector(".email-list-empty"),
            refreshEmails: document.querySelector(".email-refresh"),
            emailViewContainer: document.getElementById("app-email-view"),
            emailFilterFolderLists: Array.from(document.querySelectorAll(".email-filter-folders li")),
            emailListItemActions: Array.from(document.querySelectorAll(".email-list-item-actions li"))
        }
          , e = (r(a.emailList),
        r(a.emailFilters),
        r(a.emailView),
        (r = (e, t) => {
            e && new Quill(e,{
                modules: {
                    toolbar: t
                },
                placeholder: "Write your message...",
                theme: "snow"
            })
        }
        )(a.emailEditor, ".email-editor-toolbar"),
        r(a.emailReplyEditor, ".email-reply-toolbar"),
        a.bookmarkEmail.forEach(e => {
            e.addEventListener("click", e => {
                var t = e.currentTarget.closest(".email-list-item");
                e.stopPropagation(),
                t.hasAttribute("data-starred") ? t.removeAttribute("data-starred") : t.setAttribute("data-starred", "true")
            }
            )
        }
        ),
        a.selectAllEmails && a.selectAllEmails.addEventListener("click", t => {
            a.emailListItemInputs.forEach(e => {
                e.checked = t.currentTarget.checked
            }
            )
        }
        ),
        a.emailListItemInputs && a.emailListItemInputs.forEach(e => {
            e.addEventListener("click", e => {
                e.stopPropagation();
                var e = a.emailListItemInputs.filter(e => e.checked).length
                  , t = a.emailListItemInputs.length;
                a.selectAllEmails.indeterminate = 0 < e && e < t,
                a.selectAllEmails.checked = e === t
            }
            )
        }
        ),
        a.emailSearch && a.emailSearch.addEventListener("keyup", e => {
            let l = e.currentTarget.value.toLowerCase();
            e = document.querySelector(".email-filter-folders .active"),
            e = e ? e.getAttribute("data-target") : "inbox";
            ("inbox" !== e ? Array.from(document.querySelectorAll(`.email-list-item[data-${e}="true"]`)) : a.emailListItems).forEach(e => {
                var t = e.textContent.toLowerCase();
                e.classList.toggle("d-block", t.includes(l)),
                e.classList.toggle("d-none", !t.includes(l))
            }
            )
        }
        ),
        a.emailFilterByFolders.forEach(e => {
            e.addEventListener("click", e => {
                let l = e.currentTarget.getAttribute("data-target");
                a.appEmailSidebar.classList.remove("show"),
                a.appOverlay.classList.remove("show"),
                a.emailFilterByFolders.forEach(e => e.classList.remove("active")),
                e.currentTarget.classList.add("active"),
                a.emailListItems.forEach(e => {
                    var t = "inbox" === l || e.hasAttribute("data-" + l);
                    e.classList.toggle("d-block", t),
                    e.classList.toggle("d-none", !t)
                }
                )
            }
            )
        }
        ),
        e => {
            document.querySelector(e).classList.toggle("d-block"),
            document.querySelector(e).classList.toggle("d-none")
        }
        );
        if (a.toggleBCC && a.toggleBCC.addEventListener("click", () => e(".email-compose-bcc")),
        a.toggleCC && a.toggleCC.addEventListener("click", () => e(".email-compose-cc")),
        a.emailCompose.addEventListener("hidden.bs.modal", () => {
            document.querySelector(".email-editor .ql-editor").innerHTML = "",
            document.getElementById("emailContacts").value = "",
            s()
        }
        ),
        a.emailListDelete && a.emailListDelete.addEventListener("click", () => {
            a.emailListItemInputs.forEach(e => {
                e.checked && e.closest("li.email-list-item").remove()
            }
            ),
            a.selectAllEmails.indeterminate = !1,
            a.selectAllEmails.checked = !1,
            0 === a.emailListItems.length && a.emailListEmpty.classList.remove("d-none")
        }
        ),
        a.emailListRead && a.emailListRead.addEventListener("click", () => {
            a.emailListItemInputs.forEach(e => {
                e.checked && (e.checked = !1,
                (e = e.closest("li.email-list-item")).classList.add("email-marked-read"),
                (e = e.querySelector(".email-list-item-actions li")).classList.replace("email-read", "email-unread"),
                e.querySelector("i").classList.replace("ri-mail-open-line", "ri-mail-line"))
            }
            ),
            a.selectAllEmails.indeterminate = !1,
            a.selectAllEmails.checked = !1
        }
        ),
        a.refreshEmails && a.emailList) {
            let l = ".email-list"
              , i = new PerfectScrollbar(a.emailList,{
                wheelPropagation: !1,
                suppressScrollX: !0
            });
            a.refreshEmails.addEventListener("click", () => {
                Block.standard(l, {
                    backgroundColor: "rgba(" + window.Helpers.getCssVar("black-rgb") + ", 0.1)",
                    svgSize: "0px"
                });
                var e = document.createElement("div")
                  , t = (e.classList.add("spinner-border", "text-primary"),
                e.setAttribute("role", "status"),
                document.querySelector(".email-list .notiflix-block"));
                t && t.appendChild(e),
                setTimeout( () => {
                    i.settings.suppressScrollY = !1,
                    Block.remove(l)
                }
                , 1e3),
                i.settings.suppressScrollY = !0
            }
            )
        }
        let l = document.querySelector(".email-earlier-msgs")
          , t = (l && l.addEventListener("click", () => {
            var e = document.querySelector(".email-card-last")
              , t = l.nextElementSibling;
            e && e.classList.add("hide-pseudo"),
            t && (t.style.display = "none" !== t.style.display && t.style.display ? "none" : "block",
            t.classList.toggle("slide-toggle")),
            l.remove()
        }
        ),
        $("#emailContacts"));
        function s() {
            function e(e) {
                return e.id ? "<div class='d-flex flex-wrap align-items-center'><div class='avatar avatar-xs me-2 w-px-20 h-px-20'><img src='" + assetsPath + "img/avatars/" + $(e.element).data("avatar") + "' alt='avatar' class='rounded-circle' /></div>" + e.text + "</div>" : e.text
            }
            t.length && t.select2({
                placeholder: "Select value",
                dropdownParent: t.parent(),
                closeOnSelect: !1,
                templateResult: e,
                templateSelection: e,
                escapeMarkup: function(e) {
                    return e
                }
            })
        }
        s();
        let i = document.querySelector(".app-email-view-content");
        (r = i ? i.querySelector(".scroll-to-reply") : null) && i && r.addEventListener("click", () => {
            0 === i.scrollTop && i.scrollTo({
                top: i.scrollHeight,
                behavior: "smooth"
            })
        }
        ),
        a.emailFilterFolderLists && a.emailFilterFolderLists.forEach(e => {
            e.addEventListener("click", () => {
                a.emailViewContainer.classList.remove("show")
            }
            )
        }
        ),
        a.emailListItemActions && a.emailListItemActions.forEach(l => {
            l.addEventListener("click", e => {
                e.stopPropagation();
                var t, e = l.closest("li.email-list-item");
                l.classList.contains("email-delete") ? (e.remove(),
                document.querySelectorAll(".email-list-item").length || a.emailListEmpty.classList.remove("d-none")) : (l.classList.contains("email-read") || l.classList.contains("email-unread")) && (t = l.querySelector("i"),
                e.classList.toggle("email-marked-read", l.classList.contains("email-read")),
                l.classList.toggle("email-read"),
                l.classList.toggle("email-unread"),
                t.classList.toggle("ri-mail-open-line"),
                t.classList.toggle("ri-mail-line"))
            }
            )
        }
        )
    }
});
