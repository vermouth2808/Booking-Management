import { FacebookOutlined, InstagramOutlined, TwitterOutlined, YoutubeOutlined } from "@ant-design/icons";
import { Col, Row } from "antd";
import React from "react";
import { useSelector } from "react-redux";

const Footer = () => {
    const darkMode = useSelector((state) => state.theme.darkMode);
    const TACinemaFooter = () => {
        return (
            <footer className={`footer-app ${darkMode ? "dark" : "light"}`}>
                <span>
                    TA-Cinema - Đồng hành cùng bạn trong những thước phim
                    Bạn muốn tận hưởng những giây phút giải trí tại rạp chiếu phim nhưng không biết nên chọn bộ phim nào giữa hàng loạt tác phẩm hấp dẫn? Hãy truy cập TA-Cinema để nhanh chóng tìm được bộ phim phù hợp trước khi ra rạp!

                    Với đội ngũ giàu kinh nghiệm trong lĩnh vực điện ảnh, chúng tôi cập nhật hàng ngày những bài viết về các bom tấn đang chiếu, thông tin chi tiết về hệ thống rạp và những chương trình ưu đãi hấp dẫn dành riêng cho bạn.

                    Mỗi bài viết trên TA-Cinema đều được đầu tư kỹ lưỡng nhằm mang đến cho bạn những tin tức nóng hổi, cùng các đánh giá phim khách quan, đa chiều và chính xác nhất. Đặc biệt, tất cả thông tin đều hoàn toàn miễn phí, giúp bạn dễ dàng đưa ra quyết định nên xem phim gì.

                    Nếu bạn là một tín đồ điện ảnh, hãy ủng hộ chúng tôi bằng cách Like, Share các bài viết trên website. Sự ủng hộ của bạn sẽ là động lực để chúng tôi tiếp tục mang đến những thông tin hữu ích nhanh chóng và chất lượng hơn.

                    <br /> 📩 Mọi góp ý & liên hệ: contact@ta-cinema.com
                </span>
                <Row gutter={[16, 16]} justify="center">
                    {/* Cột 1: Danh mục phim */}
                    <Col xs={24} sm={12} md={6}>
                        <h3 className="footer-title">PHIM</h3>
                        <ul className="footer-list">
                            <li>Phim đang chiếu</li>
                            <li>Phim sắp chiếu</li>
                            <li>Phim tháng 03/2025</li>
                            <li>Phim chiếu lại</li>
                            <li>Đánh giá phim</li>
                        </ul>
                    </Col>

                    {/* Cột 2: Danh mục rạp */}
                    <Col xs={24} sm={12} md={6}>
                        <h3 className="footer-title">RẠP</h3>
                        <ul className="footer-list">
                            <li>CGV</li>
                            <li>Lotte</li>
                            <li>Galaxy</li>
                            <li>BHD</li>
                        </ul>
                    </Col>

                    {/* Cột 3: Phim sắp ra mắt */}
                    <Col xs={24} sm={12} md={6}>
                        <h3 className="footer-title">PHIM SẮP RA MẮT</h3>
                        <ul className="footer-list">
                            <li>Địa Đạo: Mặt Trời Trong Bóng Tối - 04/04/2025</li>
                            <li>Nàng Bạch Tuyết - 21/03/2025</li>
                            <li>THÁM TỬ KIẾN: KỲ ÁN KHÔNG ĐẦU - 30/04/2025</li>
                        </ul>
                    </Col>

                    {/* Cột 4: Mạng xã hội */}
                    <Col xs={24} sm={12} md={6} className="footer-social">
                        <h3 style={{ color: "#ff3377", marginBottom: "10px" }}>Kết nối với chúng tôi</h3>
                        <div>
                            <FacebookOutlined className="footer-social-icon" />
                            <TwitterOutlined className="footer-social-icon" />
                            <InstagramOutlined className="footer-social-icon" />
                            <YoutubeOutlined className="footer-social-icon" />
                        </div>
                    </Col>
                </Row>

                {/* Dòng bản quyền và điều khoản */}
                <Row justify="center" className="footer-bottom">
                    <Col xs={24}>
                        © Copyright 2025 TA-Cinema |
                        <a href="#" className="footer-link"> Privacy Policy</a> |
                        <a href="#" className="footer-link"> Terms of Use</a> |
                        <a href="#" className="footer-link"> About Us</a>
                    </Col>
                </Row>
            </footer>
        );
    };
    return (
        <TACinemaFooter></TACinemaFooter>
    );

};

export default Footer;
