#!/usr/bin/env python
import dbus
import sys

protocol = "prpl-jabber"

def help():
	print """ Icorrect parameters using
	Usage:
		main.py fromJabberName toJabberName message
	"""

def get_interface():
	try:
		obj = dbus.SessionBus().get_object("im.pidgin.purple.PurpleService" , "/im/pidgin/purple/PurpleObject")
		return dbus.Interface(obj , "im.pidgin.purple.PurpleInterface")
	except Exception , e:
		print "%s" % e
		return None

def send_message( interface , account_id , message , mailTo ):
	conv_id = interface.PurpleConversationNew(1 , account_id , mailTo)
	im = interface.PurpleConvIm(conv_id)
	interface.PurpleConvImSend(im , message)
	interface.PurpleConversationDestroy(conv_id)

def main(argv):
	if len(argv) != 4:
		help()
		return
	fromJabber = argv[1]
	toJabber = argv[2]
	message = argv[3]
	interface = get_interface()
	fromJabberId =  interface.PurpleAccountsFind(fromJabber, protocol)
	send_message(interface , fromJabberId , message , toJabber)	
	return 0

if __name__ == "__main__":
	main(sys.argv)
