import { useState, useEffect } from "react"
import { useNavigate } from "react-router-dom"
import API_ENDPOINTS from "../config/Config";
import AuthService from "../services/AuthService";
import ImageService from "../services/ImageService";
import api from '../api/api'
import { UserOutlined, LockOutlined, FacebookOutlined, GoogleOutlined } from '@ant-design/icons';
import { Form, Input, Button, Checkbox, Row, Layout } from "antd";

const LoginPage = () => {
    const [form] = Form.useForm();
    const [credentials, setCredentials] = useState({ username: "", password: "" });
    const [bgImage, setBgImage] = useState({ bannerId: null, bannerName: "", bannerUrl: "" });
    const navigate = useNavigate();
    useEffect(() => {
        const fetchImage = async () => {
            try {
                const bg = await ImageService.getBackgroundImage();
                console.log("data bg", bg)
                setBgImage(bg);
            } catch (error) {
                console.error("Error fetching background image:", error);
            }
        };
        fetchImage();
        return () => {
            setBgImage({ bannerId: null, bannerName: "", bannerUrl: "" })
        }
    }, []);

    const handleLogin = async (values) => {
        try {
            console.log("Username:", values.username);
            console.log("Password:", values.password);

            const { data } = await api.post(API_ENDPOINTS.LOGIN, values);
            console.log("Response:", data);

            console.log("Access Token:", data.token);
            console.log("Refresh Token:", data.refreshToken);

            AuthService.setAccessToken(data.token);
            AuthService.setRefreshToken(data.refreshToken);

            navigate("/home");
        } catch (error) {
            console.error("Fail login:", error.response ? error.response.data : error.message);
        }
    };




    return (
        <Layout className="login-background"
            style={{
                backgroundImage: `url("${bgImage?.bannerUrl || ""}")`
            }}
        >
            <Form onFinish={handleLogin}
                form={form}
                name="normal_login"
                className="login-form"
            >
                <Form.Item
                    name="username"
                    rules={[
                        {
                            required: true,
                            message: 'Vui lòng nhập tên đăng nhập!',
                        },
                    ]}
                >
                    <Input prefix={<UserOutlined className="site-form-item-icon" />} placeholder="Tên đăng nhập" />
                </Form.Item>
                <Form.Item
                    name="password"
                    rules={[
                        {
                            required: true,
                            message: 'Vui lòng nhập mật khẩu!',
                        },
                    ]}
                >
                    <Input
                        prefix={<LockOutlined className="site-form-item-icon" />}
                        type="password"
                        placeholder="Mật khẩu"
                    />
                </Form.Item>
                <Form.Item>
                    <Form.Item name="remember" valuePropName="checked" noStyle>
                        <Checkbox><span className="cl_w" >Ghi nhớ đăng nhập</span></Checkbox>
                    </Form.Item>

                    <a className="login-form-forgot login-form-forgot-bottom" href="">
                        Quên mật khẩu?
                    </a>
                </Form.Item>

                <Form.Item>
                    <Button
                        type="default"
                        htmlType="submit"
                        className="login-form-button icon-margin"
                        block
                    >
                        Đăng nhập
                    </Button>

                    <br />

                    <Button
                        type="primary"
                        size="middle"
                        icon={<FacebookOutlined />}
                        className="icon-margin"
                        block
                    >
                        Đăng nhập bằng Facebook
                    </Button>
                    <Button
                        type="primary"
                        size="middle"
                        className="icon-margin"
                        block
                        danger icon={<GoogleOutlined />}
                    >
                        Đăng nhập bằng Google
                    </Button>
                    <span className="cl_w">Hoặc</span> <a href="">đăng ký ngay!</a>
                </Form.Item>
            </Form>
        </Layout>



    );
};

export default LoginPage;