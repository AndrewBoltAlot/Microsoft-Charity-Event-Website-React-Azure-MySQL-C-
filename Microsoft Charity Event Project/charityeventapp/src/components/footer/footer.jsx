import React from "react";
import "./footer.css";

function Footer()
{
  return (
    <div>
      <footer>
        <div class="container bottom_border">
          <div class="container">
            <ul class="foote_bottom_ul_amrc">
              <li>
                <a href="./Home">Home</a>
              </li>
              <li>
                <a href="./About">About Us</a>
              </li>
              <li>
                <a href="./Contact">Contact Us</a>
              </li>
            </ul>
          </div>
          {/*<!--foote_bottom_ul_amrc ends here--> */}
          <p class="text-center">
            {" "}
            Copyright @2021 | Designed by{" "}
            <a id="foot-team" href="">ThunderBirds</a>
          </p>

          <ul class="social_footer_ul">

          </ul>
          {/*<!--social_footer_ul ends here-->*/}
        </div>

        <link rel="stylesheet" href="footer.css"></link>
        <link
          rel="stylesheet"
          href="https://stackpath.bootstrapcdn.com/bootstrap/4.1.3/css/bootstrap.min.css"
          integrity="sha384-MCw98/SFnGE8fJT3GXwEOngsV7Zt27NXFoaoApmYm81iuXoPkFOJwJ8ERdknLPMO"
          crossorigin="anonymous"
        ></link>
        <link
          rel="stylesheet"
          href="https://use.fontawesome.com/releases/v5.2.0/css/all.css"
          integrity="sha384-hWVjflwFxL6sNzntih27bfxkr27PmbbK/iSvJ+a4+0owXq79v+lsFkW54bOGbiDQ"
          crossorigin="anonymous"
        ></link>
      </footer>
    </div>
  );
}

export default Footer;
