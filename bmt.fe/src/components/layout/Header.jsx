import { DownOutlined, MoonOutlined, SunOutlined } from "@ant-design/icons";
import { Dropdown, Space, Switch, Typography } from "antd";
import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { toggleDarkMode } from "../../store/themeSlice";
import Category from "../../services/CategoryService";


const { Title } = Typography;
const Header = () => {
  const [categorories, setCategories] = useState([]);
  const dispatch = useDispatch();
  const darkMode = useSelector((state) => state.theme.darkMode);
  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const categorories = await Category.getAllCategory();
        console.log("API response:", categorories);
        setCategories(Array.isArray(categorories) ? categorories : []);
      } catch (error) {
        console.error("Error fetching category:", error)
        setCategories([]);
      }
    };
    fetchCategories();
  }, []);

  const items = categorories.map((category) => ({
    key: category.categoryId,
    label: category.categoryName
  }));
  return (
    <header className={`header-app ${darkMode ? "dark" : "light"}`}>
      <Title level={3} className="header-title"><span>TA-Cinema</span></Title>
      <Dropdown menu={{ items }}>
        <a>
          <Space className="dropdown">
            <span>Thể loại</span>
            <DownOutlined />
          </Space>
        </a>

      </Dropdown>
      <Switch className="header-switch"
        checked={darkMode}
        checkedChildren={<MoonOutlined />}
        unCheckedChildren={<SunOutlined />}
        onChange={() => dispatch(toggleDarkMode())}
      />
    </header>
  );
};
export default Header;
