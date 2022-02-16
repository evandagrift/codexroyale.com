import React, { useState, useContext } from "react";
import { UserContext } from "../UserContext";
import { Redirect, useParams } from "react-router-dom";
import { axios } from "../axios";
import {
  RequestResetPasswordPostAsync,
  ResetPasswordPostAsync,
} from "../Utilities/scripts";

const ForgotPasswordPage = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [password2, setPassword2] = useState("");
  const [emailSent, setEmailSent] = useState(false);
  const [passwordReset, setPasswordReset] = useState(false);
  const [redirect, setRedirect] = useState(false);
  const { passwordResetCode } = useParams();
  const { user, setUser } = useContext(UserContext);

  const requestPasswordReset = async () => {
    RequestResetPasswordPostAsync(email);
    setEmailSent(true);
  };

  const sendPasswordReset = async () => {
    if (passwordResetCode != null) {
      setPasswordReset(true);
      let fetchedUser = await ResetPasswordPostAsync(
        password,
        passwordResetCode
      );
      if (fetchedUser.username) {
        setUser(fetchedUser);
      } else setRedirect(true);
    }
  };

  let draw = "";

  if (user) {
    draw = (
      <div>
        <Redirect to="/" />
      </div>
    );
  } else {
    if (passwordResetCode != null) {
      let passwordsForm = (
        <div>
          <form onSubmit={sendPasswordReset}>
            <input
              type="password"
              className="form-control"
              placeholder="Password"
              onChange={(e) => setPassword(e.target.value)}
              required
            />
            <input
              type="password"
              className="form-control"
              placeholder="Re-Enter Password"
              onChange={(e) => setPassword2(e.target.value)}
              required
            />
          </form>
        </div>
      );

      let notMatchDraw = "";

      if (password != password2) {
        notMatchDraw = <p className="warning-text">passwords do not match</p>;
      } else {
        notMatchDraw = "";
      }

      draw = (
        <div>
          <h1>Reset Password</h1>

          {passwordsForm}
          {notMatchDraw}
          <button
            className="w-100 btn btn-lg btn-primary m-2"
            onClick={sendPasswordReset}
          >
            Reset Password
          </button>
        </div>
      );
    } else {
      if (emailSent == false) {
        draw = (
          <div>
            <h1>Forgot Password/Username</h1>
            <p>
              please enter your email and a link will be sent to recover your
              account
            </p>
            <form onSubmit={requestPasswordReset}>
              <input
                type="email"
                className="form-control"
                placeholder="Account Email"
                onChange={(e) => setEmail(e.target.value)}
                required
              />

              <button
                className="w-100 btn btn-lg btn-primary m-2"
                type="submit"
              >
                Reset Password
              </button>
            </form>
          </div>
        );
      } else {
        draw = (
          <div>
            <p>Please check your email for the recovery link</p>
            <button
              className="w-100 btn btn-lg btn-primary m-2"
              onClick={requestPasswordReset}
            >
              Resend Reset Link
            </button>
          </div>
        );
      }
    }
  }

  let redirectElement = "";
  if (redirect) {
    redirectElement = (
      <div>
        <Redirect to="/" />
      </div>
    );
  } else redirectElement = "";

  return (
    <div>
      {draw}
      {redirectElement}
    </div>
  );
};
export default ForgotPasswordPage;
