import React from "react";
import { Link, Navigate } from "react-router-dom";
import { connect } from "react-redux";
import { logIn } from "../../Actions/Account/accountActions";
import AccountRepository from "../../ApiClient/Account/AccountProxy";
import { withTranslation } from "react-i18next";
import { Button } from "react-bootstrap";
import { refreshToken as refreshTokenAction } from "../../Actions/Account/accountActions";

const mapStateToProps = (state: any) => ({
  isLoggedIn: state.common.isLoggedIn,
});

const mapDispatchToProps = (dispatch: any) => ({
  onLogIn: (
    token: string,
    refreshToken: string,
    username: string,
    userId: string,
    personGuid: string,
    roles: string[] | undefined
  ) =>
    dispatch({
      ...logIn,
      isLoggedIn: true,
      token: token,
      refreshToken: refreshToken,
      username: username,
      userId: userId,
      roles: roles,
      personGuid: personGuid,
    }),
  refreshToken: (token: string, refresh: string) =>
    dispatch({
      ...refreshTokenAction,
      token: token,
      refreshToken: refresh,
    }),
});

interface LogInProps {
  onLogIn?: (
    token: string,
    refreshToken: string,
    username: string,
    userId: string,
    personGuid: string,
    roles: string[] | undefined
  ) => {};
  refreshToken: (token: string, refresh: string) => void;
  isLoggedIn: boolean;
  t: any;
}

interface LogInState {
  username?: string;
  password?: string;
  loginFailed?: boolean;
}

class LoginForm extends React.Component<LogInProps, LogInState> {
  constructor(props: LogInProps) {
    super(props);
    this.state = {
      username: "",
      password: "",
      loginFailed: false,
    };
  }

  componentDidMount() {
    var accessToken = localStorage.getItem("access_token");
    var refresh = localStorage.getItem("refresh");
    if (accessToken && refresh) {
      AccountRepository.refreshAccessToken(accessToken, refresh).then(
        (response) => {
          const { accessToken, refreshToken } = response.data;
          this.props.refreshToken(accessToken, refreshToken);
          AccountRepository.getMe().then((getMeResponse) => {
            const { id, userName, roles, personGuid } = getMeResponse.data;
            this.props.onLogIn!(
              accessToken,
              refreshToken,
              userName,
              id,
              personGuid,
              roles
            );
            localStorage.setItem("access_token", accessToken);
            localStorage.setItem("refresh", refreshToken);
          });
        }
      );
    }
  }

  onLogIn() {
    AccountRepository.logIn({
      username: this.state.username!,
      password: this.state.password!,
    })
      .then((r: any) => {
        this.props.onLogIn!(
          r.data.access_token,
          r.data.refreshToken,
          r.data.username,
          r.data.userId,
          r.data.personGuid,
          r.data.roles
        );
        localStorage.setItem("access_token", r.data.access_token);
        localStorage.setItem("refresh", r.data.refreshToken);
      })
      .catch((r: any) => {
        this.setState({
          ...this.state,
          loginFailed: true,
        });
      });
  }

  render(): React.ReactNode {
    const { t } = this.props;
    return (
      <div className="card m-3 p-3">
        <div className="card-body">
          <div className="m-1 p-1 display-6">
            <label>{t("loging")}</label>
          </div>
          {this.state.loginFailed && (
            <div className="m-1 p-1 alert alert-danger">{t("loginFailed")}</div>
          )}
          <div className="m-1 p-1">
            <label>{t("email")}</label>
            <input
              className="form-control"
              value={this.state.username}
              onChange={(e) =>
                this.setState({
                  ...this.state,
                  username: e.target.value,
                })
              }
            ></input>
          </div>
          <div className="m-1 p-1">
            <label>{t("password")}</label>
            <input
              className="form-control"
              type="password"
              value={this.state.password}
              onChange={(e) =>
                this.setState({
                  ...this.state,
                  password: e.target.value,
                })
              }
            ></input>
          </div>
          <div className="m-1 p-1 d-flex justify-content-between">
            <div className="my-auto d-flex gap-2">
              <Link to={"register"}>{t("register")}</Link>
              <Link to={""}>{t("changePassword")}</Link>
              <Link to={""}>{t("recoverAccess")}</Link>
            </div>
            <Button
              variant="outline-primary"
              onClick={() => this.onLogIn!()}
              type="submit"
            >
              {t("logIn")}
            </Button>
          </div>
        </div>
      </div>
    );
  }
}

export default withTranslation("loginForm")(
  connect(mapStateToProps, mapDispatchToProps)(LoginForm)
);
