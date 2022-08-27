import React, { ReactElement } from 'react';
import { Link, Navigate } from "react-router-dom";
import { connect } from 'react-redux';
import { logIn } from '../../Actions/Account/accountActions';
import { useFormik } from 'formik';
import { useTranslation } from 'react-i18next';
import { Button, Row } from 'react-bootstrap';
import AccountProxy from '../../ApiClient/Account/AccountProxy';
import Swal from 'sweetalert2';
import CommonNotifications  from '../../Notifications/CommonNotifications'

const mapStateToProps = (state: any) => ({
    
});
  
const mapDispatchToProps = (dispatch: any) => ({
    
});

interface RegisterAdministratorFormProps{
  
}

interface RegisterAdministratorFormValues{
    name: string;
    addressLine1: string;
    addressLine2: string;
    city: string;
    postalCode: string;
}

const RegisterAdministratorForm = (props: RegisterAdministratorFormProps): ReactElement => {
    const {t} = useTranslation();

    const validate = (values: RegisterAdministratorFormValues)=>{
        const errors: any = {};
        if(values.addressLine1.length < 5)
            errors.addressLine1 = t('addressTooShort');
        if(values.name.length < 2)
            errors.name = t('nameTooShort');
        if(values.city.length < 3)
            errors.city = t('cityNameTooShort');
        if(values.postalCode.length < 3)
            errors.postalCode = t('postalCodeTooShort')
        return errors;
    };

    const formik = useFormik({
        initialValues: {
            name: '',
            addressLine1: '',
            addressLine2: '',
            city: '',
            postalCode: ''
        },
        validate,
        onSubmit: (values: RegisterAdministratorFormValues)=>{

        }
    });

    return (
        <div className='card m-3 p-3'>
            <Row className='text-center'>
                <div>
                    {t('createYourFirstSchool')}
                </div>
            </Row>
            <form onSubmit={formik.handleSubmit}>
                <div className='m-1 p-1'>
                    <label htmlFor='name'>{t('nameI')}</label>
                    <input className='form-control'
                        id='name'
                        name='name'
                        type='text'
                        onChange={formik.handleChange}
                        value={formik.values.name}/>
                    {formik.errors.name && formik.touched.name ? <div className='invalid-feedback d-block'>{formik.errors.name}</div> : null}
                </div>
                <div className='m-1 p-1'>
                    <label htmlFor='addressLine1'>{t('address')}</label>
                    <input className='form-control'
                        id='addressLine1'
                        name='addressLine1'
                        type='text'
                        onChange={formik.handleChange}
                        value={formik.values.addressLine1}/>
                    {formik.errors.addressLine1 && formik.touched.addressLine1 ? <div className='invalid-feedback d-block'>{formik.errors.addressLine1}</div> : null}
                </div>
                <div className='m-1 p-1'>
                    <label htmlFor='addressLine2'>{t('address')}</label>
                    <input className='form-control'
                        id='addressLine2'
                        name='addressLine2'
                        type='text'
                        onChange={formik.handleChange}
                        value={formik.values.addressLine2}/>
                    {formik.errors.addressLine2 && formik.touched.addressLine2 ? <div className='invalid-feedback d-block'>{formik.errors.addressLine2}</div> : null}
                </div>
                <div className='m-1 p-1'>
                    <label htmlFor='postalCode'>{t('postalCode')}</label>
                    <input className='form-control'
                        id='postalCode'
                        name='postalCode'
                        type='text'
                        onChange={formik.handleChange}
                        value={formik.values.postalCode}/>
                    {formik.errors.postalCode && formik.touched.postalCode ? <div className='invalid-feedback d-block'>{formik.errors.postalCode}</div> : null}
                </div>
                <div className='m-1 p-1'>
                    <label htmlFor='city'>{t('city')}</label>
                    <input className='form-control'
                        id='city'
                        name='city'
                        type='text'
                        onChange={formik.handleChange}
                        value={formik.values.city}/>
                    {formik.errors.city && formik.touched.city ? <div className='invalid-feedback d-block'>{formik.errors.city}</div> : null}
                </div>
                <div className='m-1 p-1 d-flex justify-content-end'>
                    <Button variant='outline-primary'
                        type='submit'>
                        {t('createYourFirstSchool')}
                    </Button>
                </div>
            </form>
        </div>
    );
}

export default connect(mapStateToProps, mapDispatchToProps)(RegisterAdministratorForm);
