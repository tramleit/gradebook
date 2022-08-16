import React from 'react';
import { Link, Navigate } from "react-router-dom";
import { connect } from 'react-redux';
import { logIn } from '../../Actions/Common/common';

const mapStateToProps = (state: any) => ({
      isLoggedIn: state.common.isLoggedIn
});
  
const mapDispatchToProps = (dispatch: any) => ({
    onLogIn: () => dispatch(logIn)
});

interface TimetableProps{
    onLogIn?: ()=>{},
    isLoggedIn: boolean
}

class Timetable extends React.Component<TimetableProps> {
    render(): React.ReactNode {
        return (
            <div>
                d-board Timetable
            </div>
          );
    }
}

export default connect(mapStateToProps, mapDispatchToProps)(Timetable);