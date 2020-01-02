//----------------------------------------------------------------------
// タイトル選択画面
//----------------------------------------------------------------------

//デモバーションフラグはconst.nutへ移動しました。
//const DEMOVERSISON = 0;	//デモバージョンの場合は1


//視点の種類
const VIWETYPE_FRONT = 0;
const VIWETYPE_SIDE = 1;

//ソートタイプ
const SORTTYPE_NAME = 0;
const SORTTYPE_DATE = 1;
const SORTTYPE_GENRE = 2;
const SORTTYPE_PLNUM = 3;
/*
//言語
const LANGUAGE_ENG = 0;
const LANGUAGE_SPA = 1;
const LANGUAGE_FRA = 2;
const LANGUAGE_ITA = 3;
const LANGUAGE_GER = 4;
const LANGUAGE_JPN = 5;
const LANGUAGE_CHA = 6;
const LANGUAGE_KOR = 7;
*/

//プライオリティ
const PRI_BG = -99999;
const PRI_PKG = 5;
const PRI_CSL = 6;
const PRI_TITLEBAR = 7;
const PRI_TITLEBAR_NAME = 8;
const PRI_ARROW = 9;
const PRI_DEBUG = 10;

//デモを有効にする場合は1にする
const DEMO_USE = 1;

//ビルド日時を表示する場合は1
const BUILDDATE_DIP = 0;

const PKGMOTSMOOTHING = 1;

function getMotionPath()
{
	//本体リージョンに合わせて読み込むモーションを変える
	local devid = ::get_package_dev_id();
	local path = null;
	switch( devid )
	{
	case DEVID_JP:
		switch( getPkgRegion( ) )
		{
		case PKGREGION_US:
			path = MENU_JP_TITLESELECT_US_MOTION_PATH;
			break;
		case PKGREGION_EU:
			path = MENU_JP_TITLESELECT_EU_MOTION_PATH;
			break;
		case PKGREGION_JP:
			path = MENU_JP_TITLESELECT_JP_MOTION_PATH;
			break;
		}
		break;
	case DEVID_US:
		switch( getPkgRegion( ) )
		{
		case PKGREGION_US:
			path = MENU_US_TITLESELECT_US_MOTION_PATH;
			break;
		case PKGREGION_EU:
			path = MENU_US_TITLESELECT_EU_MOTION_PATH;
			break;
		case PKGREGION_JP:
			path = MENU_US_TITLESELECT_JP_MOTION_PATH;
			break;
		}
		break;
	case DEVID_EU:
		switch( getPkgRegion( ) )
		{
		case PKGREGION_US:
			path = MENU_EU_TITLESELECT_US_MOTION_PATH;
			break;
		case PKGREGION_EU:
			path = MENU_EU_TITLESELECT_EU_MOTION_PATH;
			break;
		case PKGREGION_JP:
			path = MENU_EU_TITLESELECT_JP_MOTION_PATH;
			break;
		}
		break;
	case DEVID_AS:
		switch( getPkgRegion( ) )
		{
		case PKGREGION_US:
			path = MENU_AS_TITLESELECT_US_MOTION_PATH;
			break;
		case PKGREGION_EU:
			path = MENU_AS_TITLESELECT_EU_MOTION_PATH;
			break;
		case PKGREGION_JP:
			path = MENU_AS_TITLESELECT_JP_MOTION_PATH;
			break;
		}
		break;
	}

//	printf("s_last_language = %d path = %s\n", s_last_language, path);
	return path;
}

//設定言語を取得する
function getlastLanguage()
{
	return s_last_language;
}
//選択されたロムテーブルを取得する
function getlastIndex( index = null )
{
	if( index == null )
	{
		return s_indextable[s_last_index];
	}
	else
	{
		return s_indextable[index];
	}
}

function mode_title_select(_menu_motion = null, _start_pad_id = null)
{
::g_frameCount.logFileWrite("");
::g_frameCount.logFileWrite("app start ==========================");
::g_frameCount.logFileWrite("");

  local path = ::conv_checked_path( SCRIPT_TITLE_MODE_SELECT_PATH, ::get_package_dev_id() );
  if ( null != path ) {
    ::util_load_script(path); // タイトルスクリプト内でMenuMoteTitleSelectを置き換える運用を想定
  }

	if( s_indextable == null )
	{
		s_indextable = [];
		local i = 0;
		for ( i = 0; i < 50 ; i++ )
		{
			s_indextable.append(49);
		}
	}

	if( DEMOVERSISON == 1 )
	{
		//デモバージョンではセーブデータは初期化される。
		::g_systemdata.get_value(SystemDataValueIndex.BACKUP_FLAGS).init( ); //初期化
		::g_systemdata.get_value(SystemDataValueIndex.SRAM_DATA).init( ); //初期化
	}

	if ( s_last_index == null )
	{
		s_last_index = ::g_systemdata.get_value(SystemDataValueIndex.BACKUP_FLAGS).get_08_last_selectTitle();
	}
	if ( s_last_sortType == null)
	{
		s_last_sortType = ::g_systemdata.get_value(SystemDataValueIndex.BACKUP_FLAGS).get_07_last_sortType();
	}
	if ( s_last_viweType == null )
	{
		s_last_viweType = ::g_systemdata.get_value(SystemDataValueIndex.BACKUP_FLAGS).get_09_last_viweType();
	}
	if ( s_last_pkgregion == null )
	{
		s_last_pkgregion = PKGREGION_JP;
	}
	if ( s_last_language == null )
	{
		local devid = ::get_package_dev_id();
		switch( devid )
		{
		case DEVID_JP:
			s_last_language = LANGUAGE_JPN;
			break;
		case DEVID_US:
			s_last_language = LANGUAGE_ENG;
			break;
		case DEVID_EU:
			s_last_language = LANGUAGE_ENG;
			break;
		case DEVID_AS:
			s_last_language = LANGUAGE_ENG;
			break;
		}
	}

	if( s_menu_reboot == null )
	{
		s_menu_reboot = false;
	}

	s_ui_motionPath = conv_path(MENU_UI_TITLESELECT_MOTION_PATH);

	local devid = ::get_package_dev_id();
	printf("/// devid = %d ///\n", devid);


//	printf("get_60_firstSelectLang = %d\n", ::g_systemdata.get_value(SystemDataValueIndex.BACKUP_FLAGS).get_60_firstSelectLang( ));

	if( DEMOVERSISON == 0 )
	{
			local devid = ::get_package_dev_id();
			switch( devid )
			{
			case DEVID_JP:
//				::g_systemdata.get_value(SystemDataValueIndex.BACKUP_FLAGS).set_06_last_language(s_last_language);
				s_last_language = ::g_systemdata.get_value(SystemDataValueIndex.BACKUP_FLAGS).get_06_last_language();
				break;
			case DEVID_US:
			case DEVID_EU:
			case DEVID_AS:
				//初回起動時の言語設定
				if( ::g_systemdata.get_value(SystemDataValueIndex.BACKUP_FLAGS).get_60_firstSelectLang( ) == 0 )
				{
					::util_load_script("system/script/mode_title_select_settinglang.nut");
					::mode_title_select_settinglang_main();
					s_last_language = ::getSettingLanguage();
					::g_systemdata.get_value(SystemDataValueIndex.BACKUP_FLAGS).set_06_last_language( s_last_language );
				}
				else
				{
					s_last_language = ::g_systemdata.get_value(SystemDataValueIndex.BACKUP_FLAGS).get_06_last_language();
				}
				::g_systemdata.get_value(SystemDataValueIndex.BACKUP_FLAGS).set_60_firstSelectLang(1);
				break;
			}
			::g_systemdata.TryAutosave(true);
	}
	printf("get_60_firstSelectLang = %d\n", ::g_systemdata.get_value(SystemDataValueIndex.BACKUP_FLAGS).get_60_firstSelectLang( ));
	printf("get_06_last_language   = %d\n", ::g_systemdata.get_value(SystemDataValueIndex.BACKUP_FLAGS).get_06_last_language( ));

  local menu = MenuModeTitleSelect(_start_pad_id);

  if( ::g_menu_sound.get_bgm_id() != "bgm_title" ) {
    ::g_menu_sound.setup_bgm( "bgm_title" );
    while ( !::g_menu_sound.is_bgm_setuped() )
      wait(0);

    ::g_menu_sound.play_bgm();
    ::g_menu_sound.set_bgm_volume(BGM_TITLE_VOLUME);
  } else {
    ::g_menu_sound.pause_bgm(false);
    Sound.animateVoiceVolume("bgm_title", BGM_TITLE_VOLUME, FRAME_BGM_TITLE_FADE_OUT, 0);
  }

	menu.exec();

	return ;
}

// テスト用簡易テキストメニュータイトル選択
class MenuModeTitleSelectBase {

  m_start_pad_id = null;
  m_titles = null;
  m_menu = null;
  m_new_index = null;

  //ここを保存しておけばカーソル位置は再現されるはず
  m_current_index = null;
  m_current_sortType = null;
  m_current_viweType = null;
  m_current_pkgregion = null;
  m_current_language = null;
  //ここまで

  constructor(_start_pad_id) {
    m_start_pad_id = _start_pad_id;
    printf("MenuModeTitleSelectBase: start_pad_id = %s\n", m_start_pad_id);
    m_titles = null;
    m_menu = null;

		printf( "s_last_index = %d\n", s_last_index );

		m_current_index = s_last_index;
		m_current_sortType = s_last_sortType;
		m_current_viweType = s_last_viweType;
		m_current_pkgregion = s_last_pkgregion;
		m_current_language = s_last_language;
  }

  function exec() {
    local loop_result = false;
    this._init();
    local retry = false;
    do {
      local result = this.exec_body();

      if (result != null) {
				local wallStting = ::g_systemdata.get_value(SystemDataValueIndex.BACKUP_FLAGS).get_61_wallPaper( );	//セーブデータから取得する

				::g_wallpaper.setWallpaper(wallStting);		//壁紙設定
				::g_wallpaper.open();

				m_menu = null;							//クラスの参照を消しておく
				s_mode_title_select = null;	//クラスの参照を消しておく

        if( result == -1 )
        {
          loop_result = ::g_demo_control.exec(this);
        }
        else
        {
          m_new_index = result;
          ::util_load_script( conv_path(SCRIPT_MODE_MAIN_PATH) );
          ::mode_main_change_title(m_new_index);
          this.after_change_title();

          ::g_menu_bg.open(true);
//          ::g_menu_sound.pause_bgm(true);
          loop_result = ::mode_main();
          ::g_menu_bg.close();
        }
        if (loop_result != SessionWaitCheckResult.ACCEPT_INVITE_RESET) {
          retry = true;
        }
      }
      else {
				m_menu = null;							//クラスの参照を消しておく
				s_mode_title_select = null;	//クラスの参照を消しておく
        retry = false;
      }
    } while ( retry );
    return loop_result;
  }

  function _init() {
    local current_dev_id = ::get_current_title_dev_id();
    local title_list = ::get_package_title_dev_name_list();
    m_titles = [];
    for (local no = 0; no < title_list.len(); no++) {
      local title = ::get_title_item(title_list[no]);
      m_titles.append( ::get_item_string(title["dev_name"]) );
    }
    m_menu = this.create_menu();
    s_mode_title_select = m_menu;
  }

  // メニュー作成
  // @return メニューのinstance
  function create_menu() {}

  // 選択処理
  // @return null: 選択せず int: title_list内のindex
  function exec_body() {}

  // 選択終了してタイトル切替した後に呼ばれる処理
  // @return なし
  function after_change_title() {}

};

class MenuModeTitleSelect extends MenuModeTitleSelectBase {

  // メニュー作成
  // @return メニューのinstance
  function create_menu() {
    ::util_load_script("system/script/mode_select_list.nut");
    local current_dev_id = ::get_current_title_dev_id();
    local current_index = ::get_title_in_pack_index(current_dev_id);
    return ModeSelectList(m_titles, current_index);
  }

  // 終了
  // @return なし
  function exec_body() {
    local layer = ScaledLayer();
    layer.visible = true;
    local debugFrame = DebugVersionFrame(layer); // XXX デバッグ用バージョン表示

    local result = m_menu.exec("caption_title_select");
    return null != result ? m_menu.index : null;
  }

};

// --

// XXX タイトルメニュー差し替え用適当class
class MenuModeTitleSelect extends MenuModeTitleSelectBase {

  m_layer = null;
  m_config = null;
  m_lists = null;
  m_lists_index = 0;

  // メニュー作成
  // @return メニューのinstance
  function create_menu() {
//  SystemEtc.setClearColor(0xffffffFF);
    SystemEtc.setClearColor(0x00000000);
    m_layer = ScaledLayer();
    m_layer.visible = true;
		m_layer.priority = PRI_DEBUG;

    ::util_load_script("system/script/mode_select_list.nut");
    m_config = ::util_load_config( ::conv_path("config/title_mode_top.psb") );

    m_lists = [];
    for (local no = 0; no < m_config["items"].len(); no++) {
      m_lists.append( format( "%d. %s", no+1, m_config["items"][no]["name"]) );
    }
    local current_index = 0;

    return MenuModeTitleSelectSub(m_config, m_current_index, m_current_sortType,m_current_viweType,m_current_pkgregion,m_current_language);
//    return ModeSelectList(m_lists, current_index);
  }

  // 終了
  // @return なし
  function exec_body() {
		local debugFrame = null; // XXX デバッグ用バージョン表示
		if ( ( DEMOVERSISON == 0 ) && ( BUILDDATE_DIP == 1 ) )
		{
			debugFrame = DebugVersionFrame(m_layer); // XXX デバッグ用バージョン表示
		}

		if( m_menu != null )
		{
	    local result = m_menu.exec("caption_title_select");
			m_current_index = m_menu.m_current_index;
			m_current_sortType = m_menu.m_current_sortType;
			m_current_viweType = m_menu.m_current_viweType;
			m_current_pkgregion = m_menu.m_current_pkgregion;
			m_current_language = m_menu.m_current_language;

			s_last_index = m_current_index;
			s_last_sortType = m_current_sortType;
			s_last_viweType = m_current_viweType;
			s_last_pkgregion = m_current_pkgregion;
			s_last_language = m_current_language;

			//選択したゲームとソート、表示タイプの保存
			::g_systemdata.get_value(SystemDataValueIndex.BACKUP_FLAGS).set_07_last_sortType(s_last_sortType);
			::g_systemdata.get_value(SystemDataValueIndex.BACKUP_FLAGS).set_08_last_selectTitle(s_last_index);
			::g_systemdata.get_value(SystemDataValueIndex.BACKUP_FLAGS).set_09_last_viweType(s_last_viweType);

			::g_saveIndex = 0;	//セーブ箇所の初期化

	    if ( null != result ) {


				local index = getlastIndex( m_current_index );
//				printf( "--------------------------------------\n" );
//				printf( "getlastIndex(%d) = %d\n", m_current_index, index );
//				printf( "--------------------------------------\n" );

				local indexoffset = getLanguageIndexOffset(m_current_language);	//起動するリージョンによってオフセットを加える
	      m_lists_index = index + indexoffset;

		    printf( "m_lists_index = %d\n", m_lists_index );

				if( result == false )
				{
					// プレイデモを起動させる
					return -1;
				}

	      local item = m_config["items"][m_lists_index];
	      for (local no = 0; no < m_titles.len(); no++) {
	        if ( item["dev_name"] == m_titles[no] ) {
	          return no;
	        }
	      }
	    }
	  }
    return null;
  }

  // 選択終了してタイトル切替した後に呼ばれる処理
  // @return なし
  function after_change_title() {
    local regionTag =  m_config["items"][m_lists_index]["regionTag"];
    ::g_systemdata.get_value(SystemDataValueIndex.SETTING_ETC).set_game_regionTag(regionTag);
  }

};

function getLanguageIndexOffset( lang )
{
	local indexoffset = 0;	//起動するリージョンによってオフセットを加える
	switch( lang )
	{
		case LANGUAGE_ENG:
			indexoffset = 50 * 1;
			break;
		case LANGUAGE_SPA:
			indexoffset = 50 * 2;
			break;
		case LANGUAGE_FRA:
			indexoffset = 50 * 3;
			break;
		case LANGUAGE_ITA:
			indexoffset = 50 * 4;
			break;
		case LANGUAGE_GER:
			indexoffset = 50 * 5;
			break;
		case LANGUAGE_JPN:
			indexoffset = 50 * 0;
			break;
		case LANGUAGE_CHA:
			indexoffset = 50 * 6;
			break;
		case LANGUAGE_KOR:
			indexoffset = 50 * 7;
			break;
	}
	return ( indexoffset );
}

// --
const TITLE_NUM_JP = 42;	//タイトル数
//横と縦の数
const TITLE_FRONT_NUM_X = 6;
const TITLE_FRONT_NUM_Y_JP = 7;
const TITLE_SIDE_NUM_X = 21;
const TITLE_SIDE_NUM_Y_JP = 2;

const TITLE_NUM_US = 42;	//タイトル数
const TITLE_FRONT_NUM_Y_US = 7;
const TITLE_SIDE_NUM_Y_US = 2;

const TITLE_NUM_EU = 42;	//タイトル数
const TITLE_FRONT_NUM_Y_EU = 7;
const TITLE_SIDE_NUM_Y_EU = 2;

const TITLE_NUM_AS = 42;	//タイトル数
const TITLE_FRONT_NUM_Y_AS = 7;
const TITLE_SIDE_NUM_Y_AS = 2;

const TITLE_NUM_DEMO = 10;	//タイトル数
const TITLE_FRONT_NUM_Y_DEMO = 2;
const TITLE_SIDE_NUM_Y_DEMO = 1;



//パッケージのサイズ
const TITLE_FRONT_W = 180;
const TITLE_FRONT_H = 240;
const TITLE_SIDE_W = 50;
const TITLE_SIDE_H = 240;
//表示位置オフセット
const TITLE_FRONT_OFSX = 190;
const TITLE_FRONT_OFSY = 260;
const TITLE_SIDE_OFSX = 140;
const TITLE_SIDE_OFSY = 260;



//タイトルバー位置
const TITLEBAR_OFS_Y = 280;


function getTITLE_NUM()
{
	local devid = ::get_package_dev_id();
	local rc = null;
	switch( devid )
	{
	case DEVID_JP:
		rc = TITLE_NUM_JP;
		break;
	case DEVID_US:
		rc = TITLE_NUM_US;
		break;
	case DEVID_EU:
		rc = TITLE_NUM_EU;
		break;
	case DEVID_AS:
		rc = TITLE_NUM_AS;
		break;
	}

	if ( DEMOVERSISON == 1 )
	{
		rc = TITLE_NUM_DEMO;	//タイトル数
	}
//	printf("/// TITLE_NUM = %d ///\n", rc);
	return(rc);
}

function getTITLE_FRONT_NUM_Y()
{
	local devid = ::get_package_dev_id();
	local rc = null;
	switch( devid )
	{
	case DEVID_JP:
		rc = TITLE_FRONT_NUM_Y_JP;
		break;
	case DEVID_US:
		rc = TITLE_FRONT_NUM_Y_US;
		break;
	case DEVID_EU:
		rc = TITLE_FRONT_NUM_Y_EU;
		break;
	case DEVID_AS:
		rc = TITLE_FRONT_NUM_Y_AS;
		break;
	}
	if ( DEMOVERSISON == 1 )
	{
		rc = TITLE_FRONT_NUM_Y_DEMO;	//タイトル数
	}

	return(rc);
}

function getTITLE_SIDE_NUM_Y()
{
	local devid = ::get_package_dev_id();
	local rc = null;
	switch( devid )
	{
	case DEVID_JP:
		rc = TITLE_SIDE_NUM_Y_JP;
		break;
	case DEVID_US:
		rc = TITLE_SIDE_NUM_Y_US;
		break;
	case DEVID_EU:
		rc = TITLE_SIDE_NUM_Y_EU;
		break;
	case DEVID_AS:
		rc = TITLE_SIDE_NUM_Y_AS;
		break;
	}
	if ( DEMOVERSISON == 1 )
	{
		rc = TITLE_SIDE_NUM_Y_DEMO;	//タイトル数
	}
	return(rc);
}

// XXX タイトルメニュー差し替え用適当class
class MenuModeTitleSelectSub {

	m_rsc = null;
  m_config = null;

  m_current_index = null;
  m_current_sortType = null;
  m_current_viweType = null;
  m_current_pkgregion = null;
  m_current_language = null;

  m_layer = null;
  m_layer_button = null;

	m_motion_cur = null;	//カーソル
	m_motion_bg = null;		//背景
	m_motion_bg_button = null;		//背景
	m_motion_arrow = null;		//矢印

	m_motion_title_bar = null;		//タイトルバー

	m_motion_pkg = null;	//パッケージ
	m_motion_pkgbig = null;			//パッケージ拡大

	m_scrolloffset_y = null;	//スクロール位置
	m_keywait = null;		//キー入力ウエイト
	m_curmode = null;		//カーソルモード
	m_settingselect = null;		//設定モードのカーソル位置
	m_zoomselect = null;		//パッケージ詳細のカーソル位置

  m_debuglayer = null;
	m_debugtext = null;

  constructor(config, current_index,current_sortType,current_viweType,current_pkgregion, current_language)
  {

		m_config = config;
  	m_current_index = current_index;
		if( m_current_index >= getTITLE_NUM() )
		{
			m_current_index = getTITLE_NUM() - 1;
		}

  	m_current_sortType = current_sortType;
  	m_current_viweType = current_viweType;
	  m_current_pkgregion = current_pkgregion;
	  m_current_language = current_language;

    local motion_path = conv_path(getMotionPath());
    local font_path = conv_path(MENU_MULTI_FONT_PATH);
    local font_path2 = conv_path(MENU_MULTI_FONT12_PATH);
		local font_path3 = ::getMultiFontPath();

    m_rsc = Resource();
    m_rsc.load(motion_path, font_path, s_ui_motionPath, font_path2, font_path3);
    while (m_rsc.loading)
      wait(0);
  }

	function _init()
	{
		::g_demo_control.init();

		m_scrolloffset_y = 0;	//スクロール位置
		m_keywait = 0;		//キー入力ウエイト
		m_curmode = 0;
		m_settingselect = 0;		//設定モードのカーソル位置
		m_zoomselect = 0;		//パッケージ詳細のカーソル位置

    m_layer = ScaledLayer();
    m_layer.visible = true;
    m_layer.smoothing = false;
//    m_layer.smoothing = MOTSMOOTHING;
		m_layer.priority = PRI_BG;

		m_layer_button = ScaledLayer();
		m_layer_button.visible = true;
		m_layer_button.smoothing = MOTSMOOTHING;
		m_layer_button.priority = PRI_TITLEBAR;

		//モーションの読み込み
    local font_path = conv_path(MENU_MULTI_FONT_PATH);
    local motion_path = conv_path(getMotionPath());
/*
    local motion_path = conv_path(getMotionPath());
    local font_path = conv_path(MENU_MULTI_FONT_PATH);

    local rsc = Resource();
    rsc.load(motion_path, font_path, s_ui_motionPath);
    while (rsc.loading)
      wait(0);
*/
    m_layer.registerMotionResource(m_rsc.find(s_ui_motionPath));  // レイヤにモーションリソースを登録
    m_layer_button.registerMotionResource(m_rsc.find(s_ui_motionPath));  // レイヤにモーションリソースを登録

		//BG作成
		m_motion_bg         = Motion(m_layer);
		m_motion_bg.chara   = "bg";
		m_motion_bg.motion  = "main";
		m_motion_bg.opacity = 255;
		m_motion_bg.visible = false;
		m_motion_bg.independentLayerInherit = true;
		m_motion_bg.progress();

		m_motion_bg_button         = Motion(m_layer_button);
		m_motion_bg_button.chara   = "bg";
		m_motion_bg_button.motion  = "main_button";
		m_motion_bg_button.opacity = 255;
		m_motion_bg_button.visible = false;
		m_motion_bg_button.independentLayerInherit = true;
		m_motion_bg_button.progress();

		//タイトルバー
		m_motion_title_bar = MenuModeTitlebar(m_current_pkgregion, m_config, m_current_language, m_rsc);

		//カーソル
		m_motion_cur = MenuModeTitleCursol("cursol", null, m_config, m_rsc);
		local pos = getPkgPosition( m_current_index );
		m_motion_cur.setPosition( pos.x, pos.y );
		m_motion_cur.setLayerPriority(PRI_CSL);
		m_motion_cur.setCorsolMotion( );

		//パッケージオブジェクトの作成
    m_motion_pkg = [];
    local id = 0;
//    for ( id = 0; id < getTITLE_NUM(); id++ )
    for ( id = 0; id < 50; id++ )
    {
			local ofs = getLanguageIndexOffset( m_current_language );
	    m_motion_pkg.append( MenuModeTitleSelectPackage(m_config["items"][id + ofs]["image"], m_current_pkgregion, null, m_rsc) );
			m_motion_pkg[id].setLayerPriority(PRI_PKG);

//			printf("id = %d regionTag = %s\n", id, m_config["items"][id]["regionTag"]);
		}
		setPkgPosition( m_current_sortType );
		setViewType( m_current_viweType );

		//スクロール可能矢印
		m_motion_arrow = MenuModeTitleArrow(m_rsc);
		updateArrowVisibilities();
		updateScrollPosition();
		if( DEMOVERSISON == 1 )
		{
			m_scrolloffset_y = 0;	//デモバージョンはスクロールしない
		}
		//パッケージ詳細クラス
		m_motion_pkgbig = MenuModePkgBig( m_current_pkgregion, m_current_language, m_config, m_rsc );
		m_motion_pkgbig.setVisible( false );

/*
		//デバッグ表示
    m_debuglayer = ScaledLayer();
    m_debuglayer.visible = false;
    m_debuglayer.smoothing = MOTSMOOTHING;
    m_debuglayer.priority = PRI_DEBUG;

		m_debugtext = Indicator(m_layer, m_rsc.find(font_path));
		local _str = "test";
		m_debugtext.visible = false;
		m_debugtext.setRecognizeTag(true);
		m_debugtext.fontColor = TEXT_COLOR_NORMAL;
		m_debugtext.setAlignment(CONSOLE.ALIGNMENT_CENTER);
		m_debugtext.print(_str);
		m_debugtext.setCoord( -390, -290 );
*/
		setLanguage( m_current_language );
		setSortType( m_current_sortType );

		m_motion_bg.visible = true;
		m_motion_bg_button.visible = true;


		if( s_menu_reboot == true )
		{
			//メニュー再起動の場合は設定にカーソルを合わせておく
			m_settingselect = 0;
			m_curmode = 1;	//設定、ソート選択
			m_motion_cur.setSettingCorsol( 1 );	//カーソル形状の変更
			m_motion_cur.setVisible( false );
			m_motion_title_bar.setTitleName( -1 );
			m_motion_bg.setVariable("select", m_settingselect+1);
			m_motion_bg_button.setVariable("select", m_settingselect+1);
		}
		s_menu_reboot = false;
	}

	function updateScrollPosition()
	{
		local num = getTITLE_NUM_XY( );
		local num_x = num.x;
		local num_y = num.y;
		local x = m_current_index % num_x;
		local y = m_current_index / num_x;
		if( y >= num_y - 1 )
		{
			m_scrolloffset_y = ( -TITLE_FRONT_H * 1 ) * ( y - 1 );
		}
		else
		{
			m_scrolloffset_y = ( -TITLE_FRONT_H * 1 ) * ( y );
		}
	}

	function updateArrowVisibilities()
	{
		//上下矢印の表示を設定する
		local num = getTITLE_NUM_XY( );
		local num_x = num.x;
		local num_y = num.y;
		local x = m_current_index % num_x;
		local y = m_current_index / num_x;
		local up = false;
		local dn = false;
		if( y > 0 )
		{
			up = true;
		}
		if( y < num.y - 1 )
		{
			dn = true;
		}
		m_motion_arrow.setVisible( up, dn );
	}

	function exec( caption )
	{
    local loop_result = false;
    this._init();
		local loopexit = true;
		local reboot = false;

    do {
			//デバッグ表示
/*
			{
				//選択言語を表示する
				local _str = "1";
				switch( m_current_language )
				{
					case LANGUAGE_ENG:
						_str = "ENG";
						break;
					case LANGUAGE_SPA:
						_str = "SPA";
						break;
					case LANGUAGE_FRA:
						_str = "FRA";
						break;
					case LANGUAGE_ITA:
						_str = "ITA";
						break;
					case LANGUAGE_GER:
						_str = "GER";
						break;
					case LANGUAGE_JPN:
						_str = "JPN";
						break;
					case LANGUAGE_CHA:
						_str = "CHA";
						break;
					case LANGUAGE_KOR:
						_str = "KOR";
						break;
				}
				m_debugtext.print(_str);
			}
*/

			if( m_keywait > 0 )
			{
				m_keywait--;
			}

			if( m_curmode == 0 )	//パッケージ選択
			{
				local update = false;

				local oldindex = m_current_index;
				local num = getTITLE_NUM_XY( );
				local num_x = num.x;
				local num_y = num.y;
				local x = m_current_index % num_x;
				local y = m_current_index / num_x;

				local oldy = y
				if( m_keywait == 0 )
				{
					if ( ::g_input.key(KEY.UP) )
					{
		//				setPkgPosition( SORTTYPE_NAME );
						if( y > 0 )
						{
							y = y - 1;
							m_motion_arrow.setMoveCountUp( );
							::g_menu_sound.on_move_cursor(); // SE再生
							m_keywait = KEYWAIT;		//キー入力ウエイト
						}
						else
						{
							if( DEMOVERSISON == 0 )
							{
//								m_keywait = KEYWAIT;		//キー入力ウエイト
								m_curmode = 1;	//設定、ソート選択
								m_motion_cur.setSettingCorsol( 1 );	//カーソル形状の変更
								m_motion_cur.setVisible( false );
								m_motion_title_bar.setTitleName( -1 );
								m_motion_bg.setVariable("select", m_settingselect+1);
								m_motion_bg_button.setVariable("select", m_settingselect+1);
								::g_menu_sound.on_move_cursor(); // SE再生
							}
						}
					}
					else if ( ::g_input.key(KEY.DOWN) )
					{
		//				setPkgPosition( SORTTYPE_DATE );
						if( y < ( num_y - 1 ) )
						{
							y = y + 1;
							m_motion_arrow.setMoveCountDown( );
							::g_menu_sound.on_move_cursor(); // SE再生
							m_keywait = KEYWAIT;		//キー入力ウエイト
						}
					}
					if ( ::g_input.key(KEY.LEFT) )
					{
						::g_menu_sound.on_move_cursor(); // SE再生
						m_keywait = KEYWAIT;		//キー入力ウエイト
						if( x > 0 )
						{
							x = x - 1;
						}
						else
						{
							x = ( num_x - 1 );
						}
					}
					else if ( ::g_input.key(KEY.RIGHT) )
					{
						::g_menu_sound.on_move_cursor(); // SE再生
						m_keywait = KEYWAIT;		//キー入力ウエイト
						if( x < ( num_x - 1 ) )
						{
							x = x + 1;
							local index = x + ( y * num_x );
							if( index >= getTITLE_NUM() )
							{
								index = getTITLE_NUM() - 1;
								x = 0;
							}
						}
						else
						{
							x = 0;
						}
					}
				}
				if ( m_keywait == 0 )
				{
					if ( ::g_input.keyPressed(KEY.B) )
					{
						if( DEMOVERSISON == 0 )
						{

							::g_menu_sound.on_move_cursor(); // SE再生
	//							::g_menu_sound.on_enter(); // SE再生
			//				loopexit = false;
							//ビュータイプの変更
							if ( m_current_viweType == VIWETYPE_FRONT )
							{
								m_current_viweType = VIWETYPE_SIDE;
							}
							else
							{
								m_current_viweType = VIWETYPE_FRONT;
							}

							updateScrollPosition();
							oldy = y;

							setViewType( m_current_viweType );
							update = true;
							m_keywait = KEYWAIT;		//キー入力ウエイト
						}
					}
				}

				//カーソル位置更新
				local index = x + ( y * num_x );
				if( index >= getTITLE_NUM() )
				{
					index = getTITLE_NUM() - 1;
				}
				if( index < 0 )
				{
					index = 0;
				}
//				if ( ( oldindex != index ) || ( update == true ) )
				{
					if( oldy < y )
					{
						if( y >= 2 )
						{
							m_scrolloffset_y = ( -TITLE_FRONT_H * 1 ) * ( y - 1 );
						}
					}
					if( oldy > y )
					{
						if( y <= getTITLE_NUM_XY().y - 1 )
						{
							m_scrolloffset_y = ( -TITLE_FRONT_H * 1 ) * ( y );
						}
					}

					m_current_index = index;
					local pos = getPkgPosition( m_current_index );
					m_motion_cur.setPosition( pos.x, pos.y );
					m_motion_cur.setCorsolIndex( m_current_index, m_current_language );
	//				printf("m_motion_cur m_current_index = %d x = %f y = %f\n", m_current_index, pos.x, pos.y);
					//パッケージの位置にスクロール位置を更新
					setPkgPosition( m_current_sortType );
					updateArrowVisibilities();

					//ゲームタイトル表示
					m_motion_title_bar.setTitleName( m_current_index );

				}
				//カーソル位置を更新してから決定処理をする
				if ( m_keywait == 0 )
				{
					if ( ::g_input.keyPressed(KEY.A) )
					{
						::g_menu_sound.on_move_cursor(); // SE再生
	//						::g_menu_sound.on_enter(); // SE再生
						m_curmode = 2;	//パッケージ選択操作
						pkgSelectMode( true );	//パッケージ選択
						m_keywait = KEYWAIT;		//キー入力ウエイト
					}
				}
			}
			else if (m_curmode == 1)
			{
				//カーソルモード設定、ソート選択
				m_motion_title_bar.setTitleName( -1 );
				if( m_keywait == 0 )
				{
					if ( ::g_input.key(KEY.UP) )
					{
					}
					else if ( ::g_input.key(KEY.DOWN) )
					{
							m_motion_bg.setVariable("select", 0);
							m_motion_bg_button.setVariable("select", 0);
							m_curmode = 0;	//パッケージ選択
							m_motion_cur.setSettingCorsol( 0 );	//カーソル形状の変更
							m_motion_cur.setVisible( true );
							::g_menu_sound.on_move_cursor(); // SE再生
							m_keywait = KEYWAIT;		//キー入力ウエイト
					}
					else if ( ::g_input.key(KEY.LEFT) )
					{
						if( m_settingselect == 1 )
						{
							m_settingselect = 0
							::g_menu_sound.on_move_cursor(); // SE再生
//							m_keywait = KEYWAIT;		//キー入力ウエイト
							m_motion_bg.setVariable("select", 1);
							m_motion_bg_button.setVariable("select", 1);
						}
					}
					else if ( ::g_input.key(KEY.RIGHT) )
					{
						if( m_settingselect == 0 )
						{
							m_settingselect = 1
							::g_menu_sound.on_move_cursor(); // SE再生
//							m_keywait = KEYWAIT;		//キー入力ウエイト
							m_motion_bg.setVariable("select", 2);
							m_motion_bg_button.setVariable("select", 2);
						}
					}
				}

				if ( m_keywait == 0 )
				{
					if ( ::g_input.keyPressed(KEY.A) )
					{
						::g_menu_sound.on_enter(); // SE再生
						if( m_settingselect == 0 )
						{
	/*
							//設定を選択
							//とりあえず言語を変える
							switch( m_current_language )
							{
								case LANGUAGE_ENG:
								case LANGUAGE_SPA:
								case LANGUAGE_FRA:
								case LANGUAGE_ITA:
								case LANGUAGE_GER:
								case LANGUAGE_JPN:
								case LANGUAGE_CHA:
									m_current_language++;
									break;
								case LANGUAGE_KOR:
									m_current_language = 0;
									break;
							}
							setLanguage( m_current_language );
							setPkgPosition( m_current_sortType );
	*/
							m_motion_bg.setVariable("select", 0);
							m_motion_bg_button.setVariable("select", 0);
							s_last_language = m_current_language;
							printf("s_last_language1=%d\n", s_last_language );

							setSelectMenuPkgVisible( false );

							::util_load_script("system/script/mode_title_select_setting.nut");
							::mode_title_select_setting_main();

							local fseq = ::g_frameCount.getFinishSeq();
					    if ( fseq > 0 )
					    {
								//シャットダウンで抜けた場合はリソース読み変えずに終了する
								::g_frameCount.logFileWrite("mode_title_select_setting_main power off");
							}
							else
							{
	//							if ( m_current_pkgregion != getPkgRegion() )	//パッケージリージョンで比較してみる
								if( m_current_language != ::getSettingLanguage() )
								{
									loopexit = false;	//別の言語になったらメインループを抜ける
									reboot = true;
									s_menu_reboot = true;
									m_current_language = ::getSettingLanguage();
									break;	//直接ループを抜ける
								}
								m_current_language = ::getSettingLanguage();
								setLanguage( m_current_language );
								setPkgPosition( m_current_sortType );

								setSelectMenuPkgVisible( true );
								m_motion_bg.setVariable("select", m_settingselect+1);
								m_motion_bg_button.setVariable("select", m_settingselect+1);

								printf("s_last_language2=%d\n", s_last_language );
							}

						}
						else
						{
							//ソートモードを選択
							switch( m_current_sortType )
							{
								case SORTTYPE_NAME:
									m_current_sortType = SORTTYPE_GENRE;
									break;
								case SORTTYPE_DATE:
									m_current_sortType = SORTTYPE_NAME;
									break;
								case SORTTYPE_GENRE:
									m_current_sortType = SORTTYPE_PLNUM;
									break;
								case SORTTYPE_PLNUM:
									m_current_sortType = SORTTYPE_DATE;
									break;
							}
							setSortType( m_current_sortType );
//							m_keywait = KEYWAIT;		//キー入力ウエイト

						}
					}
				}
				//カーソル位置設定
/*
				local cx = SCREEN_XSIZE / 2;
				local cy = SCREEN_YSIZE / 2;
				if( m_settingselect == 0 )
				{
					m_motion_cur.setPosition( -446 + cx, -269 + cy );
				}
				else
				{
					m_motion_cur.setPosition(  446 + cx, -269 + cy );
				}
*/
			}
			else
			{
				if ( m_keywait == 0 )
				{
					//パッケージ選択
					if ( ::g_input.keyPressed(KEY.B) )
					{
						//キャンセル
						::g_menu_sound.on_back(); // SE再生
						m_curmode = 0;	//パッケージ選択
						pkgSelectMode( false );
						m_keywait = KEYWAIT;		//キー入力ウエイト
					}
					if ( ::g_input.keyPressed(KEY.A) )
					{
						::g_menu_sound.on_decide(); // SE再生
						//決定を選択
							m_keywait = KEYWAIT;		//キー入力ウエイト
							loopexit = false;
							//決定モーション
							m_motion_bg.setVariable("decide", 2);
							m_motion_bg_button.setVariable("decide", 2);

	          ::g_menu_sound.pause_bgm(true);	//BGMフェード
							wait(60);
					}
				}
/*
				//カーソル位置設定
				local cx = SCREEN_XSIZE / 2;
				local cy = SCREEN_YSIZE / 2;
				m_motion_cur.setPosition( 0 + cx, 194 + cy );
*/
				m_motion_pkgbig.exec();

			}
			m_motion_cur.exec();
			m_motion_arrow.exec();

			//パッケージ画像の更新
			local i = 0;
			for ( i = 0; i < m_motion_pkg.len(); i++ )
			{
				m_motion_pkg[i].exec();
			}

			::g_wallpaper.close();
			::g_wallpaperBG.close();

	    //パワーボタン検出
			local fseq = ::g_frameCount.getFinishSeq();
	    if ( fseq > 0 )
	    {
				printf("finishifunc 1\n");
				//保存する
				::g_frameCount.setFinishSeq(2);	//シャットダウン要求

				printf("finishifunc 2\n");
				::g_frameCount.appFinish();	//シャットダウン

//	      break; // モード終了
			}
			else if( DEMO_USE != 0 )
			{
				if ( DEMOVERSISON == 0 )
				{
//				if( m_curmode == 0 )	//パッケージ選択
					{
						if( ::g_demo_control.check(this) )
						{
							return( false );
						}
					}
				}
			}

      wait(0);
    } while ( loopexit );

::g_frameCount.logFileWrite("mainloop exit 1");

    if( reboot == false )
    {
::g_frameCount.logFileWrite("mainloop exit 2");
			return( true );
		}
		else
		{
::g_frameCount.logFileWrite("mainloop exit 3");
			::g_wallpaperBG.open();	//背景を残しておく
			return( null );	//再起動
		}

	}
  //ソート方法を指定してパッケージの座標を取得する
  function setPkgPosition( sortType )
  {

		local i = 0;
		for ( i = 0; i < m_motion_pkg.len(); i++ )
		{
			local index = getPkgIndex( i, sortType );
			s_indextable[index] = i;

//			printf( "s_indextable[%d] = %d\n", i, index );

			if( getTITLE_NUM() <= index )
			{
				m_motion_pkg[i].setEnable( false );
			}
			else
			{
				m_motion_pkg[i].setEnable( true );
			}

			local pos = getPkgPosition( index );
			m_motion_pkg[i].setPosition( pos.x, pos.y );
		}
	}
  //ビューの設定
  function setViewType( view )
  {

		local i = 0;
		for ( i = 0; i < m_motion_pkg.len(); i++ )
		{
			m_motion_pkg[i].setViewType( view );
		}
		m_motion_cur.setViewType( view );
	}
	//インデックスから座標を取得する
  function getPkgPosition( index )
  {
			//正面と横で座標を分ける
			local num = getTITLE_NUM_XY( );

			local w = TITLE_FRONT_W;
			local h = TITLE_FRONT_H;
			local ox = TITLE_FRONT_OFSX;
			local oy = TITLE_FRONT_OFSY;
			if ( m_current_viweType == VIWETYPE_SIDE )
			{
				w = TITLE_SIDE_W;
				h = TITLE_SIDE_H;
				ox = TITLE_SIDE_OFSX;
				oy = TITLE_SIDE_OFSY;
			}

			local x = ( ( index % num.x ) * w ) + ox;
			local y = ( ( index / num.x ) * h ) + oy + m_scrolloffset_y;
			local pos = { x = x, y = y };
			return pos;
	}

  //ソート方法を指定してパッケージのインデックスを取得する
  function getPkgIndex( id, sortType )
  {
		local ofs = getLanguageIndexOffset( m_current_language );

		local rc = 0;
		switch( sortType )
		{
		case SORTTYPE_NAME:
			//名前順
			rc = m_config["items"][id + ofs]["sor_name"];
			break;
		case SORTTYPE_DATE:
			//日付順
			rc = m_config["items"][id + ofs]["sor_date"];
			break;
		case SORTTYPE_GENRE:
			//ジャンル
			rc = m_config["items"][id + ofs]["sor_genr"];
			break;
		case SORTTYPE_PLNUM:
			//プレイ人数
			rc = m_config["items"][id + ofs]["sor_pnum"];
			break;
		}
		if( DEMOVERSISON == 1 )
		{
			//デモバージョン
			rc = m_config["items"][id + ofs]["sor_demo"];
		}
		return ( rc );
	}

	//横に並ぶ数を取得する
	function getTITLE_NUM_XY( )
	{
		local rc = { x = TITLE_FRONT_NUM_X, y = getTITLE_FRONT_NUM_Y() };
		if ( m_current_viweType == VIWETYPE_SIDE )
		{
			rc.x = TITLE_SIDE_NUM_X;
			rc.y = getTITLE_SIDE_NUM_Y();
		}
		return(rc);
	}

  //パッケージ選択操作にする
  function pkgSelectMode( flg )
  {
//    printf("pkgSelectMode = %d\n", flg);

		m_zoomselect = 0;		//パッケージ詳細のカーソル位置

		local i = 0;
		for ( i = 0; i < m_motion_pkg.len(); i++ )
		{
			if( flg == true )
			{
				m_motion_pkg[i].setVisible( false );
			}
			else
			{
				m_motion_pkg[i].setVisible( true );
			}
		}
		//BGを変更する
		if( flg == false )
		{
			m_motion_bg.motion  = "main";
			m_motion_bg_button.visible  = true;
		}
		else
		{
			m_motion_bg.motion  = "pkg_zoom";
			m_motion_bg_button.visible  = false;
		}
		//矢印非表示
		if( flg == true )
		{
			m_motion_arrow.setVisibleAll( false );
			m_motion_title_bar.setVisible( false );
//			m_debugtext.visible = false;
		}
		else
		{
			m_motion_arrow.setVisibleAll( true );
			m_motion_title_bar.setVisible( true );
//			m_debugtext.visible = true;
		}

		//カーソル形状の変更
		if( flg == true )
		{
			m_motion_cur.setVisible(false);	//カーソル
			m_motion_cur.setSettingCorsol( 2 );	//カーソル形状の変更
			m_motion_bg.setVariable("decide", 1);
			m_motion_bg_button.setVariable("decide", 1);
		}
		else
		{
			m_motion_cur.setVisible(true);	//カーソル
			m_motion_cur.setSettingCorsol( 0 );	//カーソル形状の変更
			m_motion_bg.setVariable("decide", 0);
			m_motion_bg_button.setVariable("decide", 0);
		}

		//パッケージ詳細クラス
		m_motion_pkgbig.setVisible( flg );

		//パッケージ詳細の内容を設定
		local index = m_current_index;
		m_motion_pkgbig.setTitleImage( index );

	}
	//UIすべての表示を切り替え
  function setSelectMenuVisible( flg )
  {
		m_layer.visible = flg;
		m_motion_cur.setVisible(flg);	//カーソル
		m_motion_arrow.setVisibleAll(flg);		//矢印

		m_motion_title_bar.setVisible(flg);		//タイトルバー

		local i = 0;
		for ( i = 0; i < m_motion_pkg.len(); i++ )
		{
				m_motion_pkg[i].setVisible( flg );
		}
//		m_motion_pkgbig.setVisible(flg);			//パッケージ拡大

		m_motion_cur.exec();
		m_motion_arrow.exec();

		//パッケージ画像の更新
		local i = 0;
		for ( i = 0; i < m_motion_pkg.len(); i++ )
		{
			m_motion_pkg[i].exec();
		}


//		m_debugtext.visible = false;
	}
	//UIすべての表示を切り替え
  function setSelectMenuPkgVisible( flg )
  {
//		m_layer.visible = flg;
//		m_motion_cur.setVisible(flg);	//カーソル
		m_motion_arrow.setVisibleAll(flg);		//矢印

		m_motion_title_bar.setVisible(flg);		//タイトルバー

		local i = 0;
		for ( i = 0; i < m_motion_pkg.len(); i++ )
		{
				m_motion_pkg[i].setVisible( flg );
		}
//		m_motion_pkgbig.setVisible(flg);			//パッケージ拡大

		m_motion_cur.exec();
		m_motion_arrow.exec();

		//パッケージ画像の更新
		local i = 0;
		for ( i = 0; i < m_motion_pkg.len(); i++ )
		{
			m_motion_pkg[i].exec();
		}


//		m_debugtext.visible = false;
	}

	//言語を設定する
  function setLanguage( lang )
  {
		m_current_language = lang;
		m_motion_bg.setVariable("lang", m_current_language);
		m_motion_bg_button.setVariable("lang", m_current_language);

		m_current_pkgregion = getPkgRegion( );
/*
		switch( m_current_language )
		{
			case LANGUAGE_ENG:
				m_current_pkgregion = PKGREGION_US;
				break;
			case LANGUAGE_SPA:
			case LANGUAGE_FRA:
			case LANGUAGE_ITA:
			case LANGUAGE_GER:
				m_current_pkgregion = PKGREGION_EU;
				break;
			case LANGUAGE_JPN:
			case LANGUAGE_CHA:
			case LANGUAGE_KOR:
				m_current_pkgregion = PKGREGION_JP;
				break;
		}
*/
		switch( m_current_language )
		{
			case LANGUAGE_ENG:
				::sys_set_language_tag(LANGUAGE_TAG_ENGLISH);
				break;
			case LANGUAGE_CHA:
				::sys_set_language_tag(LANGUAGE_TAG_CHINA);
				break;
			case LANGUAGE_KOR:
				::sys_set_language_tag(LANGUAGE_TAG_KOREA);
				break;
			case LANGUAGE_SPA:
				::sys_set_language_tag(LANGUAGE_TAG_SPANISH);
				break;
			case LANGUAGE_FRA:
				::sys_set_language_tag(LANGUAGE_TAG_FRENCH);
				break;
			case LANGUAGE_ITA:
				::sys_set_language_tag(LANGUAGE_TAG_ITALIAN);
				break;
			case LANGUAGE_GER:
				::sys_set_language_tag(LANGUAGE_TAG_GERMAN);
				break;
			case LANGUAGE_JPN:
				::sys_set_language_tag(LANGUAGE_TAG_JAPANESE);
				break;
		}

		//背景の変更
		m_motion_bg.chara = getBGMotionName( );
		m_motion_bg.motion  = "main";
		m_motion_bg.progress();

		m_motion_bg_button.chara = getBGMotionName( );
		m_motion_bg_button.motion  = "main_button";
		m_motion_bg_button.progress();

		//パッケージリージョンを設定する
		local i = 0;
		for ( i = 0; i < m_motion_pkg.len(); i++ )
		{
			local ofs = getLanguageIndexOffset( m_current_language );
	    m_motion_pkg[i].setPKGId( m_config["items"][i + ofs]["image"] );
			m_motion_pkg[i].setPKGRegion( m_current_pkgregion );

//			printf("id = %d regionTag = %s\n", id, m_config["items"][id]["regionTag"]);
		}
		//パッケージ詳細
		m_motion_pkgbig.setPKGRegion( m_current_pkgregion );
		m_motion_pkgbig.setLanguage( m_current_language );
		//タイトル表示
		m_motion_title_bar.setPKGRegion( m_current_pkgregion );
		m_motion_title_bar.setLanguage( m_current_language );
	}
	//ソートタイプの変更
	function setSortType( sortType )
	{
		m_current_sortType = sortType;
		s_last_sortType = sortType;
		setPkgPosition( m_current_sortType );
		m_motion_bg.setVariable("sort", m_current_sortType);
		m_motion_bg_button.setVariable("sort", m_current_sortType);
	}



};


//タイトルパッケージクラス
class MenuModeTitleSelectPackage
{
	m_rsc = null;
	m_motion = null;
	m_layer = null;
	m_devid = null;
	m_config = null;

	m_viewType = null;

	m_posx = null;
	m_posy = null;

	m_targetposx = null;
	m_targetposy = null;

	m_dir = null;
	m_spd = null;

	m_alpha = null;
	m_seq = null;
	m_pkgregion = null;

	m_isCursor = null;

	m_isVisible = null;
	m_isEnable = null;

  constructor( id, pkgregion = null, config = null, rcs = null )
  {
		m_rsc = rcs;
		m_pkgregion = pkgregion;

		m_viewType = VIWETYPE_FRONT;
		m_isCursor = false;
		m_isVisible = true;
		m_isEnable = true;

    m_layer = ScaledLayer();
    m_layer.visible = true;
//    m_layer.smoothing = MOTSMOOTHING;
    m_layer.smoothing = PKGMOTSMOOTHING;
    local motion_path = conv_path(getMotionPath());
/*
    local rsc = Resource();
    rsc.load(motion_path);
    while (rsc.loading)
      wait(0);
*/
    m_layer.registerMotionResource(m_rsc.find(motion_path));  // レイヤにモーションリソースを登録

		m_devid = id;

		m_alpha = 255;

		m_posx = 0;
		m_posy = 0;

		m_targetposx = 0;
		m_targetposy = 0;

		m_dir = 0;
		m_spd = 0;

		m_seq = 0;

  	_init( );
  }

  function exec()
  {
		switch( m_seq )
		{
			case 0:
				//移動
				//自機に向かってホーミング
				m_dir = PointDir( m_posx, m_posy, m_targetposx, m_targetposy );
				m_spd = TragetKinspeed_Option( m_posx, m_posy, m_targetposx, m_targetposy );
				local pos = DirMove( m_dir, m_spd, m_posx, m_posy );
				m_posx = pos.x;
				m_posy = pos.y;

				if( m_spd == 0 )
				{
					m_posx = m_targetposx;
					m_posy = m_targetposy;
					m_seq++;
				}

				break;
			default:
				//移動終了
				m_posx = m_targetposx;
				m_posy = m_targetposy;
				m_spd = 0;
				break;
		}

		//表示座標更新
		m_motion.left = m_posx;
		m_motion.top = m_posy;

		m_motion.visible = true;

		local addalpha = 16;
		if( m_isCursor == false )
		{
			//画面外の表示を消す
			local cx = SCREEN_XSIZE / 2;
			local cy = SCREEN_YSIZE / 2;
			if ( ( ( m_targetposy + cy ) < TITLE_FRONT_OFSY ) || ( ( m_targetposy + cy )  > ( TITLE_FRONT_OFSY + ( TITLE_FRONT_H * 1 ) ) ) )
			{
				m_alpha = m_alpha - addalpha;
				if( m_alpha < 0 )
				{
					m_alpha = 0;
					m_motion.visible = false;
				}
				m_motion.opacity = m_alpha;
			}
			else
			{
				m_alpha = m_alpha + addalpha;
				if( m_alpha > 255 )
				{
					m_alpha = 255;
				}
				m_motion.visible = true;
				m_motion.opacity = m_alpha;
			}
		}
		if( m_isVisible == false )
		{
				m_motion.visible = false;
		}
		if( m_isEnable == false )
		{
				m_motion.visible = false;
		}
  }

  function _init()
  {
		m_motion         = Motion(m_layer);
		m_motion.chara   = "pkg";

		if( m_pkgregion != null )
		{
			if( m_pkgregion == PKGREGION_JP )
			{
				m_motion.motion  = "front";
			}
			if( m_pkgregion == PKGREGION_US )
			{
				m_motion.motion  = "front_us";
			}
			if( m_pkgregion == PKGREGION_EU )
			{
				m_motion.motion  = "front_eu";
			}
		}
		m_motion.opacity = 255;
		m_motion.visible = true;
		m_motion.independentLayerInherit = true;
		m_motion.progress();
		m_motion.left = 0;	//0,0が画面中央
		m_motion.top = 0;
		m_motion.setVariable("pkg", m_devid);
  }

  function setPosition( x, y )
  {
		local cx = SCREEN_XSIZE / 2;
		local cy = SCREEN_YSIZE / 2;
		m_targetposx = x - cx;
		m_targetposy = y - cy;
		m_seq = 0;
  }
  //------------------------------
  // レイヤのプライオリティを設定
  //------------------------------
  function setLayerPriority(_priority) {
    if (m_layer != null) m_layer.priority = _priority;
  }

  function setVisible( flg )
  {
		m_isVisible = flg;
//    printf("setVisible = %d\n", m_isVisible);
  }
  function setEnable( flg )
  {
		m_isEnable = flg;
  }


	//表示タイプ
  function setViewType( view )
  {
		m_viewType = view;
		if ( view == VIWETYPE_SIDE )
		{
			if( m_pkgregion != null )
			{
				if( m_pkgregion == PKGREGION_JP )
				{
					m_motion.motion  = "side";
				}
				if( m_pkgregion == PKGREGION_US )
				{
					m_motion.motion  = "side_us";
				}
				if( m_pkgregion == PKGREGION_EU )
				{
					m_motion.motion  = "side_eu";
				}
			}
		}
		else
		{
			if( m_pkgregion != null )
			{
				if( m_pkgregion == PKGREGION_JP )
				{
					m_motion.motion  = "front";
				}
				if( m_pkgregion == PKGREGION_US )
				{
					m_motion.motion  = "front_us";
				}
				if( m_pkgregion == PKGREGION_EU )
				{
					m_motion.motion  = "front_eu";
				}
			}
		}
  }

  //パッケージIDの更新
  function setPKGId( id )
  {
		m_devid = id;
		m_motion.setVariable("pkg", m_devid);
	}

	//パッケージリージョンの設定
  function setPKGRegion( pkgregion )
  {
		m_pkgregion = pkgregion;
		setViewType( m_viewType );
	}

  //カーソルのみで使用
  //カーソル用のモーションか？
  function setCorsolMotion( )
  {
		m_isCursor = true;
	}
  function setCorsolIndex( index, lang )
  {
		local tblIndex = getlastIndex( index ) + getLanguageIndexOffset(lang);
		local size = m_config["items"][tblIndex]["csize"];
		m_motion.setVariable( "size", size );

//		printf( "index = d, size = %d\n", tblIndex, size );
	}
  //設定ボタン用のモーションにするか？
  function setSettingCorsol( flg )
  {
		if( flg == 1 )
		{
			m_motion.motion  = "button";
		}
		else if( flg == 2 )
		{
			m_motion.motion  = "pkgzoom_button";
		}
		else
		{
			if( m_viewType == VIWETYPE_FRONT )
			{
				m_motion.motion  = "front";
			}
			else
			{
				m_motion.motion  = "side";
			}
		}
	}
}

//カーソルクラス
class MenuModeTitleCursol extends MenuModeTitleSelectPackage
{
  constructor( id, pkgregion = null, config = null, rsc = null )
  {
		m_rsc = rsc;
		m_config = config
		m_pkgregion = pkgregion;

		m_viewType = VIWETYPE_FRONT;
		m_isCursor = false;
		m_isVisible = true;

    m_layer = ScaledLayer();
    m_layer.visible = true;
    m_layer.smoothing = MOTSMOOTHING;
/*
    local rsc = Resource();
    rsc.load(s_ui_motionPath);
    while (rsc.loading)
      wait(0);
*/
    m_layer.registerMotionResource(m_rsc.find(s_ui_motionPath));  // レイヤにモーションリソースを登録

		m_devid = id;

		m_alpha = 255;

		m_posx = 0;
		m_posy = 0;

		m_targetposx = 0;
		m_targetposy = 0;

		m_dir = 0;
		m_spd = 0;

		m_seq = 0;

  	_init( );
  }

  function _init()
  {
		m_motion         = Motion(m_layer);
		m_motion.chara   = "cursol";

		m_motion.motion  = "front";
		m_motion.opacity = 255;
		m_motion.visible = true;
		m_motion.independentLayerInherit = true;
		m_motion.progress();
		m_motion.left = 0;	//0,0が画面中央
		m_motion.top = 0;
  }
	//表示タイプ
  function setViewType( view )
  {
		m_viewType = view;
		if ( view == VIWETYPE_SIDE )
		{
			m_motion.motion  = "side";
		}
		else
		{
			m_motion.motion  = "front";
		}
  }
}

//スクロール矢印クラス
const ARROW_OFFSET_Y = 20;
const ARROW_OFFSET_X = 560;
const ARROW_OFFSET_H = 200;
class MenuModeTitleArrow
{

	m_rsc = null;
	m_motion_up = null;
	m_motion_down = null;
	m_layer = null;

	m_movecount_up = null;
	m_movecount_down = null;

	m_alpha = null;
	m_seq = null;
	m_isVisible = null;

  constructor( rsc )
  {
		m_rsc = rsc;
		m_isVisible = true;

    m_layer = ScaledLayer();
    m_layer.visible = true;
    m_layer.smoothing = MOTSMOOTHING;
		m_layer.priority = PRI_ARROW;
/*
    local rsc = Resource();
    rsc.load(s_ui_motionPath);
    while (rsc.loading)
      wait(0);
*/
    m_layer.registerMotionResource(m_rsc.find(s_ui_motionPath));  // レイヤにモーションリソースを登録


		m_alpha = 255;

		m_movecount_up = 0;
		m_movecount_down = 0;

  	_init( );
  }

  function exec()
  {
		if ( m_movecount_up > 0 )
		{
			m_movecount_up--;
		}
		if ( m_movecount_down > 0 )
		{
			m_movecount_down--;
		}

		//表示座標更新
//		m_motion_up.left = m_posx;
		local ofs = (m_movecount_up * m_movecount_up) / 10;
		m_motion_up.top = -ARROW_OFFSET_H + ARROW_OFFSET_Y - ofs;

//		m_motion_down.left = m_posx;
		ofs = (m_movecount_down * m_movecount_down) / 10;
		m_motion_down.top = ARROW_OFFSET_H + ARROW_OFFSET_Y + ofs;

		if( m_isVisible == false )
		{
			m_motion_up.visible = false;
			m_motion_down.visible = false;
		}

	}
  function _init()
  {
		m_motion_up         = Motion(m_layer);
		m_motion_up.chara   = "bg";
		m_motion_up.motion  = "arrow_up";
		m_motion_up.opacity = 255;
		m_motion_up.visible = true;
		m_motion_up.independentLayerInherit = true;
		m_motion_up.progress();
		m_motion_up.left = ARROW_OFFSET_X;	//0,0が画面中央
		m_motion_up.top = -ARROW_OFFSET_H + ARROW_OFFSET_Y;

		m_motion_down         = Motion(m_layer);
		m_motion_down.chara   = "bg";
		m_motion_down.motion  = "arrow_down";
		m_motion_down.opacity = 255;
		m_motion_down.visible = true;
		m_motion_down.independentLayerInherit = true;
		m_motion_down.progress();
		m_motion_down.left = ARROW_OFFSET_X;	//0,0が画面中央
		m_motion_down.top = ARROW_OFFSET_H + ARROW_OFFSET_Y;
  }

  //矢印の表示を設定
  function setVisible( up, down )
  {
		m_motion_up.visible = up;
		m_motion_down.visible = down;
	}
  function setVisibleAll( flg )
  {
		m_isVisible = flg;
	}
  //矢印の動きを設定
  function setMoveCountUp( )
  {
		m_movecount_up = 10;
	}
  function setMoveCountDown( )
  {
		m_movecount_down = 10;
	}

}

//タイトル名表示クラス
const TITLEBAR_OFFSET_Y = 310;
const TITLEBAR_TEXT_OFFSET_Y = 294;
const TITLEBAR_ICON1_OFFSET_X = 420;
const TITLEBAR_ICON2_OFFSET_X = 490;
const TITLEBAR_ICON_OFFSET_Y = 310;
const INDEX_LENGTH_MAX = 700;
class MenuModeTitlebar
{
	m_rsc = null

	m_layer_bg = null;
	m_layer = null;
	m_motion_bar = null;
	m_motion_name = null;
	m_motion_pnum = null;
	m_motion_genre = null;
	m_text_Title = null;

	m_language = null;
	m_pkgregion = null;
	m_isVisible = null;

	m_index = null;
	m_config = null;


  constructor( pkgregion, config, language, rsc )
  {
		m_rsc = rsc;
		m_language = language
		m_config = config;
		m_pkgregion = pkgregion;
		m_isVisible = true;
		m_index = 0;

    m_layer = ScaledLayer();
    m_layer.visible = true;
//    m_layer.smoothing = MOTSMOOTHING;
    m_layer.smoothing = true;
		m_layer.priority = PRI_TITLEBAR_NAME;

    m_layer_bg = ScaledLayer();
    m_layer_bg.visible = true;
    m_layer_bg.smoothing = MOTSMOOTHING;
		m_layer_bg.priority = PRI_TITLEBAR;

    local motion_path = conv_path(getMotionPath());
/*
    local rsc = Resource();
    rsc.load(motion_path, s_ui_motionPath);
    while (rsc.loading)
      wait(0);
*/
    m_layer.registerMotionResource(m_rsc.find(motion_path));  // レイヤにモーションリソースを登録
    m_layer_bg.registerMotionResource(m_rsc.find(s_ui_motionPath));  // レイヤにモーションリソースを登録

    local font_path = ::getTitleMultiFontPath( );
//    local font_path = conv_path(MENU_MULTI_FONT_PATH);

    local rsc = Resource();
    rsc.load(font_path);
    while (rsc.loading)
      wait(0);
		m_text_Title = Indicator(m_layer, rsc.find(font_path));
		local _str = "";
		m_text_Title.visible = true;
		m_text_Title.setRecognizeTag(true);
		m_text_Title.fontColor = TEXT_COLOR_NORMAL;
		m_text_Title.setAlignment(CONSOLE.ALIGNMENT_CENTER);
		m_text_Title.print(_str);
		m_text_Title.setCoord( 0, TITLEBAR_TEXT_OFFSET_Y );
//		m_text_Title.setFontScale( 1 );

  	_init( );
  }

  function exec()
  {

	}
  function _init()
  {
		m_motion_bar         = Motion(m_layer_bg);
		m_motion_bar.chara   = "bg";
		m_motion_bar.motion  = "title_bar";
		m_motion_bar.opacity = 255;
		m_motion_bar.visible = true;
		m_motion_bar.independentLayerInherit = true;
		m_motion_bar.progress();
		m_motion_bar.left = 0;	//0,0が画面中央
		m_motion_bar.top = TITLEBAR_OFFSET_Y;

		m_motion_name         = Motion(m_layer);
		m_motion_name.chara   = "pkg";
		m_motion_name.motion  = "name";
		m_motion_name.opacity = 255;
		m_motion_name.visible = false;
		m_motion_name.independentLayerInherit = true;
		m_motion_name.progress();
		m_motion_name.left = 0;	//0,0が画面中央
		m_motion_name.top = TITLEBAR_OFFSET_Y;

		m_motion_pnum         = Motion(m_layer);
		m_motion_pnum.chara   = "pkg";
		m_motion_pnum.motion  = "big_pnum";
		m_motion_pnum.opacity = 255;
		m_motion_pnum.visible = false;
		m_motion_pnum.independentLayerInherit = true;
		m_motion_pnum.progress();
		m_motion_pnum.left = TITLEBAR_ICON1_OFFSET_X;	//0,0が画面中央
		m_motion_pnum.top = TITLEBAR_ICON_OFFSET_Y;

		m_motion_genre         = Motion(m_layer);
		m_motion_genre.chara   = "pkg";
		m_motion_genre.motion  = "big_genre";
		m_motion_genre.opacity = 255;
		m_motion_genre.visible = false;
		m_motion_genre.independentLayerInherit = true;
		m_motion_genre.progress();
		m_motion_genre.left = TITLEBAR_ICON2_OFFSET_X;	//0,0が画面中央
		m_motion_genre.top = TITLEBAR_ICON_OFFSET_Y;

  }

  function setTitleName( index )
  {
		m_index = index;
		if( index == -1 )
		{
			m_motion_name.visible = false;
			m_motion_pnum.visible = false;
			m_motion_genre.visible = false;
			m_text_Title.visible = false;
		}
		else
		{
			local tblIndex = getlastIndex( index );
//			printf( "--------------------------------------\n" );
//			printf( "getlastIndex(%d) = %d\n", index, tblIndex );
//			printf( "--------------------------------------\n" );


			local lid = m_config["items"][tblIndex]["image"];

			m_motion_name.chara   = "pkg";

			switch ( m_language )
			{
			case LANGUAGE_JPN:
				m_motion_name.motion  = "name";
				break;
			case LANGUAGE_ENG:
				m_motion_name.motion  = "name_us";
				break;
			case LANGUAGE_SPA:
			case LANGUAGE_FRA:
			case LANGUAGE_ITA:
			case LANGUAGE_GER:
				m_motion_name.motion  = "name_eu";
				break;
			case LANGUAGE_CHA:
				m_motion_name.motion  = "name_ch";
				break;
			case LANGUAGE_KOR:
				m_motion_name.motion  = "name_ko";
				break;
			}

			m_motion_name.setVariable("pkg", lid);
//			m_motion_name.visible = true;


			switch( m_pkgregion )
			{
			case PKGREGION_JP:
				m_motion_bar.chara   = "bg";
				break;
			case PKGREGION_US:
				m_motion_bar.chara   = "bg_us";
				break;
			case PKGREGION_EU:
				m_motion_bar.chara   = "bg_eu";
				break;
			}
			m_motion_bar.motion  = "title_bar";

			m_motion_pnum.visible = true;
			m_motion_pnum.setVariable("pkg", lid);
			m_motion_genre.visible = true;
			m_motion_genre.setVariable("pkg", lid);

			//タイトルフォントサイズ設定
			local ofs = getLanguageIndexOffset( m_language );
			local tname = m_config["items"][tblIndex + ofs]["tname"];
			m_text_Title.visible = true;
			m_text_Title.print(tname);
			m_text_Title.setFontScaleX( 1 );
			local scale = 1;
			local width = m_text_Title.width;
			if( INDEX_LENGTH_MAX < width )
			{
				scale = scale * INDEX_LENGTH_MAX / width;
				m_text_Title.setFontScaleX(scale);
			}


		}
		if( m_isVisible == false )
		{
			m_text_Title.visible = false;
			m_motion_name.visible = false;
			m_motion_pnum.visible = false;
			m_motion_genre.visible = false;
		}

	}
  function setVisible( flg )
  {
		m_isVisible = flg;
		m_motion_bar.visible = flg;
//		m_motion_name.visible = flg;
		m_motion_pnum.visible = flg;
		m_motion_genre.visible = flg;
		m_text_Title.visible = flg;
  }
  function setPKGRegion( pkgregion )
  {
		m_pkgregion = pkgregion;
		setTitleName( m_index );
	}
	function setLanguage( language )
	{
		m_language = language;
		setTitleName( m_index );
	}
}
//パッケージ詳細表示クラス
const PKGBIG_OFFSET_Y = -25;
const PKGBIG_OFFSET_W = -255;
const PKGBIGNAME_OFFSET_Y = -191;
const PKGBIGNAME_OFFSET_W = 200;
const PKGBIGNAME_TEXTOFFSET_Y = -206;

const PKGBIGTEXT_OFFSET_X = -150;
const PKGBIGTEXT_OFFSET_Y = -153;

const PKGBIGCOPY_OFFSET_X = -150;
//const PKGBIGCOPY_OFFSET_Y = 115;
const PKGBIGCOPY_OFFSET_Y = 80;

const PKGBIG3BPAD_OFFSET_X = -210;
const PKGBIG3BPAD_OFFSET_Y = 135;
const PAD3B_LENGTH_MAX_JP = 550;
const PAD3B_LENGTH_MAX_EN = 620;

const PKGBIGICON_OFFSET_Y = 135;
const PKGBIGICON1_OFFSET_X = -316;
const PKGBIGICON2_OFFSET_X = -246;

class MenuModePkgBig
{
	m_rsc = null;
	m_layer = null;
	m_layer_pkg = null;
	m_motion_pkg = null;
	m_motion_text = null;
	m_motion_name = null;
	m_motion_pnum = null;
	m_motion_genre = null;

	m_pkgregion = null;
	m_language = null;
	m_isVisible = null;

	m_index = null;
	m_config = null;
	m_text_Description = null;
	m_text_Copy = null;
	m_text_Title = null;
	m_text_3bpad = null;

	m_configguide = null;
	m_guide = null;

  constructor( pkgregion, language, config, rsc )
  {
		m_rsc = rsc;
		m_config = config;
		m_index = 0;
		m_pkgregion = pkgregion;
		m_language = language;
		m_isVisible = true;

    m_layer = ScaledLayer();
    m_layer.visible = true;
    m_layer.smoothing = MOTSMOOTHING;
		m_layer.priority = PRI_TITLEBAR;

    m_layer_pkg = ScaledLayer();
    m_layer_pkg.visible = true;
    m_layer_pkg.smoothing = PKGMOTSMOOTHING;
		m_layer_pkg.priority = PRI_TITLEBAR;

    local motion_path = conv_path(getMotionPath());
//    local font_path = conv_path(MENU_MULTI_FONT_PATH);
		local font_path = ::getMultiFontPath();
    local font_path2 = conv_path(MENU_MULTI_FONT12_PATH);
		local font_path3 = ::getTitleMultiFontPath( );

    local rsc = Resource();
    rsc.load(font_path, font_path3);
    while (rsc.loading)
      wait(0);

		m_layer.registerMotionResource(m_rsc.find(motion_path));  // レイヤにモーションリソースを登録
		m_layer_pkg.registerMotionResource(m_rsc.find(motion_path));  // レイヤにモーションリソースを登録

		m_text_Description = Indicator(m_layer, rsc.find(font_path));
		local _str = "";
		m_text_Description.visible = false;
		m_text_Description.setRecognizeTag(true);
		m_text_Description.fontColor = TEXT_COLOR_NORMAL;
		m_text_Description.setAlignment(CONSOLE.ALIGNMENT_LEFT);
		m_text_Description.print(_str);
		m_text_Description.setCoord( PKGBIGTEXT_OFFSET_X, PKGBIGTEXT_OFFSET_Y );

		m_text_Copy = Indicator(m_layer_pkg, m_rsc.find(font_path2));
		local _str = "";
		m_text_Copy.visible = false;
		m_text_Copy.setRecognizeTag(true);
		m_text_Copy.fontColor = TEXT_COLOR_NORMAL;
//		m_text_Copy.setAlignment(CONSOLE.ALIGNMENT_CENTER);
		m_text_Copy.print(_str);
		m_text_Copy.setCoord( PKGBIGCOPY_OFFSET_X, PKGBIGCOPY_OFFSET_Y );
//		m_text_Copy.setFontScale( 0.5 );

		m_text_Title = Indicator(m_layer, rsc.find(font_path3));
		local _str = "";
		m_text_Title.visible = true;
		m_text_Title.setRecognizeTag(true);
		m_text_Title.fontColor = TEXT_COLOR_NORMAL;
		m_text_Title.setAlignment(CONSOLE.ALIGNMENT_CENTER);
		m_text_Title.print(_str);
		m_text_Title.setCoord( 0, PKGBIGNAME_TEXTOFFSET_Y );
//		m_text_Title.setFontScale( 1 );

    m_configguide = ::util_load_config( ::conv_path("config/mode_title_select.psb") );
/*
		local textData = m_configguide["PKGBIG"];
		local str = get_item_string(textData["SettingMainText__GUIDE"]);
		m_text_guide = Indicator(m_layer, rsc.find(font_path));
		m_text_guide.visible = true;
		m_text_guide.setRecognizeTag(true);
		m_text_guide.fontColor = TEXT_COLOR_NORMAL;
		m_text_guide.setAlignment(CONSOLE.ALIGNMENT_CENTER);
		m_text_guide.print(str);
		m_text_guide.setCoord( 0, 253 - 16 );
*/

		local textData = m_configguide["3BPAD"];
		local str = get_item_string(textData["SettingMainText__GUIDE"]);
		m_text_3bpad = Indicator(m_layer, rsc.find(font_path));
//		m_text_3bpad = Indicator(m_layer, m_rsc.find(font_path2));
		m_text_3bpad.visible = false;
		m_text_3bpad.setRecognizeTag(true);
		m_text_3bpad.fontColor = TEXT_COLOR_NORMAL;
		m_text_3bpad.setAlignment(CONSOLE.ALIGNMENT_LEFT);
		m_text_3bpad.print(str);
		m_text_3bpad.setCoord( PKGBIG3BPAD_OFFSET_X, PKGBIG3BPAD_OFFSET_Y );

  	_init( );
  }

  function exec()
  {
    if ( ::g_input.key(KEY.X) )	// 3Buttonの時に表示
		{
//			printf( "3button on\n")
			m_text_3bpad.visible = m_isVisible;
		}
		else
		{
//			printf( "3button off\n")
			m_text_3bpad.visible = false;
		}
	}
  function _init()
  {
		m_motion_pkg         = Motion(m_layer_pkg);
		m_motion_pkg.chara   = "pkg";
		m_motion_pkg.motion  = "big";
		m_motion_pkg.opacity = 255;
		m_motion_pkg.visible = true;
		m_motion_pkg.independentLayerInherit = true;
		m_motion_pkg.progress();
		m_motion_pkg.left = PKGBIG_OFFSET_W;	//0,0が画面中央
		m_motion_pkg.top = PKGBIG_OFFSET_Y;
/*
		m_motion_text         = Motion(m_layer);
		m_motion_text.chara   = "pkg";
		m_motion_text.motion  = "big_text_c";
		m_motion_text.opacity = 255;
		m_motion_text.visible = false;
		m_motion_text.independentLayerInherit = true;
		m_motion_text.progress();
		m_motion_text.left = PKGBIGCOPY_OFFSET_X;	//0,0が画面中央
		m_motion_text.top = PKGBIGCOPY_OFFSET_Y;
*/
		m_motion_name         = Motion(m_layer);
		m_motion_name.chara   = "pkg";
		m_motion_name.motion  = "name";
		m_motion_name.opacity = 255;
		m_motion_name.visible = false;
		m_motion_name.independentLayerInherit = true;
		m_motion_name.progress();
		m_motion_name.left = 0;	//0,0が画面中央
		m_motion_name.top = PKGBIGNAME_OFFSET_Y;

		m_motion_pnum         = Motion(m_layer);
		m_motion_pnum.chara   = "pkg";
		m_motion_pnum.motion  = "big_pnum";
		m_motion_pnum.opacity = 255;
		m_motion_pnum.visible = false;
		m_motion_pnum.independentLayerInherit = true;
		m_motion_pnum.progress();
		m_motion_pnum.left = PKGBIGICON1_OFFSET_X;	//0,0が画面中央
		m_motion_pnum.top = PKGBIGICON_OFFSET_Y;

		m_motion_genre         = Motion(m_layer);
		m_motion_genre.chara   = "pkg";
		m_motion_genre.motion  = "big_genre";
		m_motion_genre.opacity = 255;
		m_motion_genre.visible = false;
		m_motion_genre.independentLayerInherit = true;
		m_motion_genre.progress();
		m_motion_genre.left = PKGBIGICON2_OFFSET_X;	//0,0が画面中央
		m_motion_genre.top = PKGBIGICON_OFFSET_Y;

		m_guide = null;
		local textDataGuide = m_configguide["GUIDE_MAIN"];
		local guidenum = 1;
		if( textDataGuide )
		{
			m_guide = [];

			local i = 0;
			for ( i = 0; i < guidenum ; i++ )
			{
				local idxname = "SettingMainIcon__" + i.tostring();
				local str1 = get_item_string(textDataGuide[idxname]);

				idxname = "SettingMainText__" + i.tostring();
				local str2 = get_item_string(textDataGuide[idxname]);

				local x = 0;
				local w = 0;
				m_guide.append( MenuModeGuide(m_layer, x + w * i, 253, str1, str2 ) );

			}
		}
  }

  function setTitleImage( index )
  {
		m_index = index;

		local tblIndex = getlastIndex( index );
//		printf( "--------------------------------------\n" );
//		printf( "getlastIndex(%d) = %d\n", index, tblIndex );
//		printf( "--------------------------------------\n" );
		local ofs = getLanguageIndexOffset( m_language );
		local lid = m_config["items"][tblIndex + ofs]["image"];

		{
			m_motion_pkg.chara   = "pkg";
			switch( m_pkgregion )
			{
			case PKGREGION_JP:
				m_motion_pkg.motion  = "big";
				break;
			case PKGREGION_US:
				m_motion_pkg.motion  = "big_us";
				break;
			case PKGREGION_EU:
				m_motion_pkg.motion  = "big_eu";
				break;
			}

			switch ( m_language )
			{
			case LANGUAGE_JPN:
				m_motion_name.motion  = "name";
				break;
			case LANGUAGE_ENG:
				m_motion_name.motion  = "name_us";
				break;
			case LANGUAGE_SPA:
			case LANGUAGE_FRA:
			case LANGUAGE_ITA:
			case LANGUAGE_GER:
				m_motion_name.motion  = "name_eu";
				break;
			case LANGUAGE_CHA:
				m_motion_name.motion  = "name_ch";
				break;
			case LANGUAGE_KOR:
				m_motion_name.motion  = "name_ko";
				break;
			}


			m_motion_pkg.visible = true;
			m_motion_pkg.setVariable("pkg", lid);

			local size = m_config["items"][tblIndex + ofs]["csize"];
			if(size == 1 )
			{
				//ファンタジーゾーン
				m_motion_pkg.left = PKGBIG_OFFSET_W + getLangOffset()-25;
				m_motion_pkg.top = PKGBIG_OFFSET_Y - 40;
			}
			else
			{
				//通常パッケージ
				m_motion_pkg.left = PKGBIG_OFFSET_W + getLangOffset();
				m_motion_pkg.top = PKGBIG_OFFSET_Y;
			}


//			m_motion_name.visible = true;
			m_motion_name.setVariable("pkg", lid);
			m_motion_pnum.visible = true;
			m_motion_pnum.setVariable("pkg", lid);
			m_motion_genre.visible = true;
			m_motion_genre.setVariable("pkg", lid);

			m_text_Description.visible = true;

			local ofs = getLanguageIndexOffset( m_language );
			local _str = m_config["items"][tblIndex + ofs]["desc"];
			m_text_Description.print(_str);
/*
			m_motion_text.chara   = "pkg";

			m_motion_text.motion  = "big_text_c";
			m_motion_text.visible = true;
			m_motion_text.setVariable("pkg", lid);
*/
			m_text_Copy.visible = true;
			local cp = m_config["items"][tblIndex]["copy"];
			m_text_Copy.print(cp);


			m_text_Title.visible = true;
			//タイトルフォントサイズ設定
			local tname = m_config["items"][tblIndex + ofs]["tname"];
			m_text_Title.visible = true;
			m_text_Title.print(tname);
			m_text_Title.setFontScaleX( 1 );
			local scale = 1;
			local width = m_text_Title.width;
			if( INDEX_LENGTH_MAX < width )
			{
				scale = scale * INDEX_LENGTH_MAX / width;
				m_text_Title.setFontScaleX(scale);
			}

			local textData = m_configguide["3BPAD"];
			local str = get_item_string(textData["SettingMainText__GUIDE"]);
//			printf("3BPAD = %s", str );

//			m_text_3bpad.visible = false;
			m_text_3bpad.print(str);
			m_text_3bpad.setFontScaleX( 1 );
			local scale = 1;
			local width = m_text_3bpad.width;
			if ( getLangOffset() != 0 )
			{
				if( PAD3B_LENGTH_MAX_JP < width )
				{
					scale = scale * PAD3B_LENGTH_MAX_JP / width;
					m_text_3bpad.setFontScaleX(scale);
				}
			}
			else
			{
				if( PAD3B_LENGTH_MAX_EN < width )
				{
					scale = scale * PAD3B_LENGTH_MAX_EN / width;
					m_text_3bpad.setFontScaleX(scale);
				}
			}

			m_guide = null;
			local textDataGuide = m_configguide["GUIDE_MAIN"];
			local guidenum = 1;
			if( textDataGuide )
			{
				m_guide = [];

				local i = 0;
				for ( i = 0; i < guidenum ; i++ )
				{
					local idxname = "SettingMainIcon__" + i.tostring();
					local str1 = get_item_string(textDataGuide[idxname]);

					idxname = "SettingMainText__" + i.tostring();
					local str2 = get_item_string(textDataGuide[idxname]);

					local x = 0;
					local w = 0;
					m_guide.append( MenuModeGuide(m_layer, x + w * i, 253, str1, str2 ) );

				}
			}

//			m_text_guide.visible = true;

		}
		if( m_isVisible == false )
		{
			m_motion_pkg.visible = false;
//			m_motion_text.visible = false;
			m_text_Copy.visible = false;
//			m_motion_name.visible = false;
			m_text_Description.visible = false;
			m_motion_pnum.visible = false;
			m_motion_genre.visible = false;
			m_text_Title.visible = false;
//			m_text_3bpad.visible = false;
//			m_text_guide.visible = false;
			for( local i = 0; i < m_guide.len(); i++ )
			{
				m_guide[i].setVisible( false );
			}
		}

	}
  function setVisible( flg )
  {
		m_isVisible = flg;
		m_motion_pkg.visible = flg;
//		m_motion_text.visible = flg;
		m_text_Copy.visible = flg;
//		m_motion_name.visible = flg;
		m_text_Description.visible = flg;
		m_motion_pnum.visible = flg;
		m_motion_genre.visible = flg;
		m_text_Title.visible = flg;
//		m_text_3bpad.visible = flg;
//		m_text_guide.visible = flg;
		for( local i = 0; i < m_guide.len(); i++ )
		{
			m_guide[i].setVisible( flg );
		}
/*
		local tblIndex = getlastIndex( m_index );
		local ofs = getLanguageIndexOffset( m_language );
		local size = m_config["items"][tblIndex + ofs]["csize"];
		if(size == 1 )
		{
			//ファンタジーゾーン
			m_motion_pkg.left = PKGBIG_OFFSET_W + getLangOffset();
			m_motion_pkg.top = PKGBIG_OFFSET_Y - 40;
		}
		else
		{
			//通常パッケージ
			m_motion_pkg.left = PKGBIG_OFFSET_W + getLangOffset();
			m_motion_pkg.top = PKGBIG_OFFSET_Y;
		}
*/
//		m_motion_text.left = PKGBIGCOPY_OFFSET_X + getLangOffset();
		m_text_Copy.setCoord( PKGBIGCOPY_OFFSET_X + getLangOffset(), PKGBIGCOPY_OFFSET_Y );
		m_motion_pnum.left = PKGBIGICON1_OFFSET_X + getLangOffset();
		m_motion_genre.left = PKGBIGICON2_OFFSET_X + getLangOffset();
		m_text_Description.setCoord( PKGBIGTEXT_OFFSET_X + getLangOffset(), PKGBIGTEXT_OFFSET_Y );
		m_text_3bpad.setCoord( PKGBIG3BPAD_OFFSET_X + getLangOffset(), PKGBIG3BPAD_OFFSET_Y );
  }
  function setPKGRegion( pkgregion )
  {
		m_pkgregion = pkgregion;
		setTitleImage( m_index );
	}
	function setLanguage( language )
	{
		if( m_language != language )
		{
			//言語に合わせたフォントの変更
			m_text_Description = null;

			local font_path = ::getMultiFontPath();
			local rsc = Resource();
			rsc.load(font_path);
			while (rsc.loading)
				wait(0);

			m_text_Description = Indicator(m_layer, rsc.find(font_path));
			local _str = "";
			m_text_Description.visible = m_isVisible;
			m_text_Description.setRecognizeTag(true);
			m_text_Description.fontColor = TEXT_COLOR_NORMAL;
			m_text_Description.setAlignment(CONSOLE.ALIGNMENT_LEFT);
			m_text_Description.print(_str);
			m_text_Description.setCoord( PKGBIGTEXT_OFFSET_X, PKGBIGTEXT_OFFSET_Y );

			m_text_3bpad = Indicator(m_layer, rsc.find(font_path));
			m_text_3bpad.visible = false;
			m_text_3bpad.setRecognizeTag(true);
			m_text_3bpad.fontColor = TEXT_COLOR_NORMAL;
			m_text_3bpad.setAlignment(CONSOLE.ALIGNMENT_LEFT);
			m_text_3bpad.print(_str);
			m_text_3bpad.setCoord( PKGBIG3BPAD_OFFSET_X, PKGBIG3BPAD_OFFSET_Y );

		}
		m_language = language;
		setTitleImage( m_index );

	}
	function getLangOffset()
	{
		local rc = 0;
		switch( m_language )
		{
		case LANGUAGE_JPN:
		case LANGUAGE_CHA:
			rc = 70;
			break;
		}

		return(rc);
	}
}
//ガイド表示クラス
class MenuModeGuide
{
	m_icon = null;
	m_text = null;

	constructor( layer, posx, posy, icon, text, width = 180 )
	{

		local font_path = ::getMultiFontPath();

		local rsc = Resource();
		rsc.load(font_path);
		while (rsc.loading)
			wait(0);

		m_icon = Indicator(layer, rsc.find(font_path));
		m_icon.visible = true;
		m_icon.setRecognizeTag(true);
		m_icon.fontColor = TEXT_COLOR_NORMAL;
//		m_icon.setAlignment(CONSOLE.ALIGNMENT_CENTER);
		m_icon.print(icon);
		m_icon.setCoord( posx, posy - 16 );

		m_text = Indicator(layer, rsc.find(font_path));
		m_text.visible = true;
		m_text.setRecognizeTag(true);
		m_text.fontColor = TEXT_COLOR_NORMAL;
//		m_text.setAlignment(CONSOLE.ALIGNMENT_CENTER);
		m_text.print(text);
		m_text.setCoord( posx + 32, posy - 12 );

		local scale = 1;
		if( width )
		{
			local width1 = m_text.width;
			if( width < width1 )
			{
				scale = scale * width / width1;
				m_text.setFontScaleX(scale);
			}
		}

		local width2 = m_text.width;
		local cx = ( 40 + width2 ) / 2;

		m_icon.setCoord( posx - cx, posy - 16 );
		m_text.setCoord( posx + 32 - cx, posy - 12 );

  }

  //
  function setVisible( flg )
  {
		m_icon.visible = flg;
		m_text.visible = flg;
	}

}

/*
// koizumi add
const MAX_DIR = 1024;	//角度
// 円周率
const M_PI = 3.141592;
const PI_2 = 6.283184;

//ユーティリティ
function DirToDeg( dir )
{
	local rc = dir * PI_2 / MAX_DIR;

	return( rc );
}
//移動後の座標を取得
function DirMove( dir, spd, x, y )
{
	local move_x = 0;
	local move_y = 0;

	local rot = DirToDeg(dir);
	dir = dir % MAX_DIR;

	move_x = (  sin( rot ) * spd );
	move_y = ( -cos( rot ) * spd );


	local position = { x = x + move_x, y = y + move_y };
	return( position );

}

function getRund( max )
{
	local val = rand() % max;
	return val;
}

//目的地の角度を取得
function PointDir( sx, sy, ex, ey )
{
	local rc = 0;

	local px = ex - sx;
	local py = ey - sy;

	local dir = atan2(px, -py);
	local rot = dir * MAX_DIR / PI_2;
	rc = rot;

	return( rc );
}


//絶対値を求める
function absf( val )
{
	if ( val < 0 )
	{
		val = -val;
	}
	return( val );
}

//近似値を求める
function Kinjiti( x1, y1, x2, y2 )
{
	local rc = 0;

	local absx = absf( x1 - x2 );
	local absy = absf( y1 - y2 );

	if ( absx > absy )
	{
		rc = ( absx + absy ) - ( absy / 2.0 );
	}
	else
	{
		rc = ( absx + absy ) - ( absx / 2.0 );
	}

	return( rc );
}

const OPTKINSPD_MAX = 9;
function TragetKinspeed_Option(start_x, start_y, end_x, end_y)
{

	local kinspdOptionX = [ 256, 128, 64, 32, 16, 8, 4, 2, 0];
	local kinspdOptionY = [ 128,  64, 32, 16,  8, 4, 2, 0, 0];

	local rc = 0;
	local sa = Kinjiti(start_x, start_y, end_x, end_y);
	local i;
	for (i = 0; i < OPTKINSPD_MAX; i++)
	{
		if (kinspdOptionX[i] < absf(sa))
		{
			rc = kinspdOptionY[i];
			if (sa < 0)
			{
				rc = -rc;
			}
			break;
		}
	}

	return (rc);
}
*/



