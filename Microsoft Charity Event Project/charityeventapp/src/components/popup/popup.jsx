import React from "react";
import styles from "./popup.css"

const Popup = (props) =>
{

  return (
    <div className="popup-box">
      <div className="pbox poverlay-container">
        <span className="close" onClick={() => { props.closePopup(false); }}>x</span>
        {props.content}
      </div>
    </div>
  );
};



export default Popup;