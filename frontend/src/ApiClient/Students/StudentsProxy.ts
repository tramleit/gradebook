import axios, { AxiosResponse } from "axios";
import { axiosApiAuthorized } from "../AxiosInterceptor";
import NewStudentRequest from "./Definitions/NewStudentRequest";

const API_URL = process.env.REACT_APP_API_URL;

const addNewStudent = (student: NewStudentRequest): Promise<AxiosResponse<any>> => {
    return axiosApiAuthorized.post(API_URL + '/students', student);
};


export default {
    addNewStudent
}