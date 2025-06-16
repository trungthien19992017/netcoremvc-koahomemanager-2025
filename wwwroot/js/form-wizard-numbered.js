( () => {
    var e = document.querySelector(".wizard-modern-example")
      , l = [].slice.call(e.querySelectorAll(".btn-next"))
      , r = [].slice.call(e.querySelectorAll(".btn-prev"))
      , c = e.querySelector(".btn-submit");
    //if (null !== e) {
    //    let t = new Stepper(e,{
    //        linear: !1
    //    });
    //    l && l.forEach(e => {
    //        e.addEventListener("click", e => {
    //            t.next()
    //        }
    //        )
    //    }
    //    ),
    //    r && r.forEach(e => {
    //        e.addEventListener("click", e => {
    //            t.previous()
    //        }
    //        )
    //    }
    //    ),
    //    c && c.addEventListener("click", e => {
    //        alert("Submitted..!!")
    //    }
    //    )
    //}
    //if (e = document.querySelector(".wizard-vertical"),
    //l = [].slice.call(e.querySelectorAll(".btn-next")),
    //r = [].slice.call(e.querySelectorAll(".btn-prev")),
    //c = e.querySelector(".btn-submit"),
    //null !== e) {
    //    let t = new Stepper(e,{
    //        linear: !1
    //    });
    //    l && l.forEach(e => {
    //        e.addEventListener("click", e => {
    //            t.next()
    //        }
    //        )
    //    }
    //    ),
    //    r && r.forEach(e => {
    //        e.addEventListener("click", e => {
    //            t.previous()
    //        }
    //        )
    //    }
    //    ),
    //    c && c.addEventListener("click", e => {
    //        alert("Submitted..!!")
    //    }
    //    )
    //}
    //if (e = document.querySelector(".wizard-modern-example"),
    //l = [].slice.call(e.querySelectorAll(".btn-next")),
    //r = [].slice.call(e.querySelectorAll(".btn-prev")),
    //c = e.querySelector(".btn-submit"),
    //null !== e) {
    //    let t = new Stepper(e,{
    //        linear: !1
    //    });
    //    l && l.forEach(e => {
    //        e.addEventListener("click", e => {
    //            t.next()
    //        }
    //        )
    //    }
    //    ),
    //    r && r.forEach(e => {
    //        e.addEventListener("click", e => {
    //            t.previous()
    //        }
    //        )
    //    }
    //    ),
    //    c && c.addEventListener("click", e => {
    //        alert("Submitted..!!")
    //    }
    //    )
    //}
    if (e = document.querySelector(".wizard-modern-vertical"),
    l = [].slice.call(e.querySelectorAll(".btn-next")),
    r = [].slice.call(e.querySelectorAll(".btn-prev")),
    c = e.querySelector(".btn-submit"),
    null !== e) {
        let t = new Stepper(e,{
            linear: !1
        });
        l && l.forEach(e => {
            e.addEventListener("click", e => {
                t.next()
            }
            )
        }
        ),
        r && r.forEach(e => {
            e.addEventListener("click", e => {
                t.previous()
            }
            )
        }
        ),
        c && c.addEventListener("click", e => {
            alert("Submitted..!!")
        }
        )
    }
}
)();
