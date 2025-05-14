import { DownOutlined, MoonOutlined, SunOutlined } from "@ant-design/icons";
import { Dropdown, Space, Switch, Typography } from "antd";
import React, { useEffect, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { toggleDarkMode } from "../../store/themeSlice";
import { RootState } from "../../store";
import Category from "../../services/CategoryService";
import { Link } from "react-router-dom";
import { CategoryModelRes } from "../../models/CategoryModelRes";

const { Title } = Typography;
const Header = () => {
    
  const [categorories, setCategories] = useState<CategoryModelRes[]>([]);
  const dispatch = useDispatch();
  const darkMode = useSelector((state:RootState ) => state.theme.darkMode);
  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const categorories = await Category.getAllCategory();
        setCategories(Array.isArray(categorories) ? categorories : []);
      } catch (error) {
        console.error("Error fetching category:", error)
        setCategories([]);
      }
    };
    fetchCategories();
  }, []);

  const items = categorories?.map((category) => ({
    key: category.categoryId,
    label: category.categoryName
  }));
  return (
    <header className={`header-app ${darkMode ? "dark" : "light"}`}>
      <Link to={`/home`}  className="header-title"><Title level={3}><span >TA-Cinema</span></Title></Link>
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
