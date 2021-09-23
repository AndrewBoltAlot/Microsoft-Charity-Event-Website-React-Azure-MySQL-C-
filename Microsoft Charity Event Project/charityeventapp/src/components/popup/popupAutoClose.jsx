import React from "react";
import styles from "./popup.css"



const PopupAutoClose = (props) =>
{
  setTimeout(function ()
  {
    props.closePopup(false)
  }, 2000)
  return (
    <div className="popup-box">
      <div className="pbox poverlay-container">
        <span className="close" onClick={() => { props.closePopup(false); }}>x</span>
        {props.content}
      </div>
    </div>
  );
};


export default PopupAutoClose;